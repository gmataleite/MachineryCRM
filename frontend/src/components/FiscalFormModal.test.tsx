import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { render, screen, waitFor, cleanup, fireEvent } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { FiscalFormModal } from './FiscalFormModal';
import * as customerService from '../services/customerService';

vi.mock('../services/customerService');
vi.mock('@vis.gl/react-google-maps', () => ({
  APIProvider: ({ children }: { children: React.ReactNode }) => children,
  useMapsLibrary: () => null
}));

describe('FiscalFormModal', () => {
  const defaultProps = { isOpen: true, onClose: vi.fn(), onSuccess: vi.fn(), customerId: '123' };

  let consoleErrorSpy: ReturnType<typeof vi.spyOn>;

  beforeEach(() => {
    vi.clearAllMocks();
    // Silencia o console.error para não poluir o terminal com os erros simulados
    consoleErrorSpy = vi.spyOn(console, 'error').mockImplementation(() => {});
  });
  afterEach(() => {
    cleanup();
    // Restaura o console.error original após cada teste
    consoleErrorSpy.mockRestore();
  });

  it('1. Renderização do modal de criação', () => {
    render(<FiscalFormModal {...defaultProps} />);
    expect(screen.getByText(/Novo Ente Fiscal/i)).toBeInTheDocument();
  });

  it('2. Razão Social vazia impede submissão', async () => {
    const { container } = render(<FiscalFormModal {...defaultProps} />);
    fireEvent.submit(container.querySelector('form')!);

    expect(await screen.findByText('A Razão Social é obrigatória.')).toBeInTheDocument();
    expect(customerService.addFiscalEntityToCustomer).not.toHaveBeenCalled();
  });

  it('3. TaxId vazio envia payload com taxId: null', async () => {
    vi.mocked(customerService.addFiscalEntityToCustomer).mockResolvedValueOnce({} as never);
    const { container } = render(<FiscalFormModal {...defaultProps} />);

    await userEvent.type(screen.getByPlaceholderText(/Razão Social/i), 'Empresa Sem Doc');
    fireEvent.submit(container.querySelector('form')!);

    await waitFor(() => {
      expect(customerService.addFiscalEntityToCustomer).toHaveBeenCalledWith(
        '123',
        expect.objectContaining({
          name: 'Empresa Sem Doc',
          taxId: null
        })
      );
    });
  });

  it('4. CPF brasileiro válido envia payload formatado', async () => {
    vi.mocked(customerService.addFiscalEntityToCustomer).mockResolvedValueOnce({} as never);
    const { container } = render(<FiscalFormModal {...defaultProps} />);

    await userEvent.type(screen.getByPlaceholderText(/Razão Social/i), 'Pessoa Física');
    await userEvent.type(screen.getByPlaceholderText(/CNPJ ou CPF/i), '529.982.247-25');

    fireEvent.submit(container.querySelector('form')!);

    await waitFor(() => {
      expect(customerService.addFiscalEntityToCustomer).toHaveBeenCalledWith(
        '123',
        expect.objectContaining({
          taxId: { value: '52998224725', countryCode: 'BR' }
        })
      );
    });
  });

  it('6. Documento inválido exibe mensagem e bloqueia API', async () => {
    const { container } = render(<FiscalFormModal {...defaultProps} />);

    await userEvent.type(screen.getByPlaceholderText(/Razão Social/i), 'Empresa Teste');
    await userEvent.type(screen.getByPlaceholderText(/CNPJ ou CPF/i), '11111111111');

    fireEvent.submit(container.querySelector('form')!);

    expect(
      await screen.findByText('Documento fiscal inválido para o país informado.')
    ).toBeInTheDocument();
    expect(customerService.addFiscalEntityToCustomer).not.toHaveBeenCalled();
  });

  it('8 & 9. Endereço de entrega espelhado ou separado baseado no checkbox', async () => {
    vi.mocked(customerService.addFiscalEntityToCustomer).mockResolvedValueOnce({} as never);
    const { container } = render(<FiscalFormModal {...defaultProps} />);

    await userEvent.type(screen.getByPlaceholderText(/Razão Social/i), 'Teste Endereço');

    const addressInputs = screen.getAllByPlaceholderText(/Sua Cidade/i);
    await userEvent.type(addressInputs[0], 'Bauru');

    fireEvent.submit(container.querySelector('form')!);

    await waitFor(() => {
      expect(customerService.addFiscalEntityToCustomer).toHaveBeenCalledWith(
        '123',
        expect.objectContaining({
          billingAddress: expect.objectContaining({ city: 'Bauru' }),
          shippingAddress: expect.objectContaining({ city: 'Bauru' })
        })
      );
    });
  });

  it('10. Atualização usa updateFiscalEntity', async () => {
    vi.mocked(customerService.updateFiscalEntity).mockResolvedValueOnce({} as never);
    const initialData = {
      id: 'fisc-1',
      name: 'Matriz',
      taxId: { value: '11222333000181', countryCode: 'BR' },
      billingAddress: { city: 'São Paulo' },
      shippingAddress: { city: 'São Paulo' }
    };

    const { container } = render(
      <FiscalFormModal {...defaultProps} initialData={initialData as never} />
    );
    fireEvent.submit(container.querySelector('form')!);

    await waitFor(() => {
      expect(customerService.updateFiscalEntity).toHaveBeenCalledWith('fisc-1', expect.anything());
    });
  });

  it('11. Erro de serviço exibe mensagem e não fecha modal', async () => {
    vi.mocked(customerService.addFiscalEntityToCustomer).mockRejectedValueOnce(
      new Error('Server Error')
    );
    const { container } = render(<FiscalFormModal {...defaultProps} />);

    await userEvent.type(screen.getByPlaceholderText(/Razão Social/i), 'Erro Teste');
    fireEvent.submit(container.querySelector('form')!);

    expect(await screen.findByText(/Erro ao salvar entidade fiscal/i)).toBeInTheDocument();
    expect(defaultProps.onClose).not.toHaveBeenCalled();
  });
});
