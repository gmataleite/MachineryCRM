import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { render, screen, waitFor, cleanup, fireEvent } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { ContactFormModal } from './ContactFormModal';
import * as customerService from '../services/customerService';

vi.mock('../services/customerService');

describe('ContactFormModal', () => {
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
    render(<ContactFormModal {...defaultProps} />);
    expect(screen.getByText(/Novo Contato/i)).toBeInTheDocument();
  });

  it('2. Nome vazio impede submissão e exibe erro', async () => {
    const { container } = render(<ContactFormModal {...defaultProps} />);
    fireEvent.submit(container.querySelector('form')!);

    expect(await screen.findByText(/O nome do contato é obrigatório/i)).toBeInTheDocument();
    expect(customerService.addContactToCustomer).not.toHaveBeenCalled();
  });

  it('3. Payload de criação enviado corretamente e sucesso chama callbacks', async () => {
    vi.mocked(customerService.addContactToCustomer).mockResolvedValueOnce({} as never);
    const { container } = render(<ContactFormModal {...defaultProps} />);

    await userEvent.type(screen.getByPlaceholderText(/João, Gerente/i), ' João Silva ');
    await userEvent.type(screen.getByPlaceholderText(/contato@empresa.com.br/i), 'joao@teste.com');
    await userEvent.type(screen.getByPlaceholderText(/\(14\) 99999-0000/i), '11999999999');
    await userEvent.type(
      screen.getByPlaceholderText(/responsável por compras/i),
      'Contato principal'
    );

    fireEvent.submit(container.querySelector('form')!);

    await waitFor(() => {
      expect(customerService.addContactToCustomer).toHaveBeenCalledWith(
        '123',
        expect.objectContaining({
          name: 'João Silva',
          email: 'joao@teste.com',
          phone: '11999999999',
          observations: 'Contato principal',
          siteId: null,
          fiscalEntityId: null
        })
      );
    });

    expect(defaultProps.onSuccess).toHaveBeenCalled();
    expect(defaultProps.onClose).toHaveBeenCalled();
  });

  it('4. Campos opcionais vazios são omitidos corretamente', async () => {
    vi.mocked(customerService.addContactToCustomer).mockResolvedValueOnce({} as never);
    const { container } = render(<ContactFormModal {...defaultProps} />);

    await userEvent.type(screen.getByPlaceholderText(/João, Gerente/i), 'Maria');
    fireEvent.submit(container.querySelector('form')!);

    await waitFor(() => {
      expect(customerService.addContactToCustomer).toHaveBeenCalledWith(
        '123',
        expect.objectContaining({
          name: 'Maria',
          email: undefined,
          phone: undefined,
          observations: undefined
        })
      );
    });
  });

  it('6. Atualização chama o serviço correto com initialData', async () => {
    vi.mocked(customerService.updateContact).mockResolvedValueOnce({} as never);
    const initialData = { id: 'cont-1', name: 'Antigo', email: '', phone: '', observations: '' };

    const { container } = render(<ContactFormModal {...defaultProps} initialData={initialData} />);

    const input = screen.getByDisplayValue('Antigo');
    await userEvent.clear(input);
    await userEvent.type(input, 'Novo Nome');

    fireEvent.submit(container.querySelector('form')!);

    await waitFor(() => {
      expect(customerService.updateContact).toHaveBeenCalledWith(
        'cont-1',
        expect.objectContaining({
          name: 'Novo Nome'
        })
      );
    });
  });

  it('8. Erro do serviço exibe mensagem e modal permanece aberto', async () => {
    vi.mocked(customerService.addContactToCustomer).mockRejectedValueOnce(
      new Error('Network error')
    );
    const { container } = render(<ContactFormModal {...defaultProps} />);

    await userEvent.type(screen.getByPlaceholderText(/João, Gerente/i), 'Erro Teste');
    fireEvent.submit(container.querySelector('form')!);

    expect(await screen.findByText(/Erro ao/i)).toBeInTheDocument();
    expect(defaultProps.onClose).not.toHaveBeenCalled();
    expect(defaultProps.onSuccess).not.toHaveBeenCalled();
  });
});
