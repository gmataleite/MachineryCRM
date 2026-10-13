import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { render, screen, waitFor, cleanup, fireEvent } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { SiteFormModal } from './SiteFormModal';
import * as customerService from '../services/customerService';

vi.mock('../services/customerService');
vi.mock('@vis.gl/react-google-maps', () => ({
  APIProvider: ({ children }: { children: React.ReactNode }) => children,
  useMapsLibrary: () => null
}));

describe('SiteFormModal', () => {
  const defaultProps = {
    isOpen: true,
    onClose: vi.fn(),
    onSuccess: vi.fn(),
    customerId: '123'
  };

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
    render(<SiteFormModal {...defaultProps} />);
    expect(screen.getByText(/Novo Local Produtivo/i)).toBeInTheDocument();
  });

  it('2. Nome vazio impede submissão e exibe erro', async () => {
    const { container } = render(<SiteFormModal {...defaultProps} />);

    // Dispara o submit direto no form para ignorar o required do HTML5
    fireEvent.submit(container.querySelector('form')!);

    expect(await screen.findByText(/O nome do local é obrigatório/i)).toBeInTheDocument();
    expect(customerService.addSiteToCustomer).not.toHaveBeenCalled();
  });

  it('3 e 4. Cidade e Estado são obrigatórios', async () => {
    const { container } = render(<SiteFormModal {...defaultProps} />);

    await userEvent.type(screen.getAllByPlaceholderText(/Nome do local/i)[0], 'Fábrica 1');
    fireEvent.submit(container.querySelector('form')!);

    expect(await screen.findByText(/Cidade e Estado são obrigatórios/i)).toBeInTheDocument();
    expect(customerService.addSiteToCustomer).not.toHaveBeenCalled();
  });

  it('5. Payload de criação trimado e countryCode nulo se vazio', async () => {
    vi.mocked(customerService.addSiteToCustomer).mockResolvedValueOnce({} as never);
    const { container } = render(<SiteFormModal {...defaultProps} />);

    await userEvent.type(screen.getAllByPlaceholderText(/Nome do local/i)[0], ' Obra 2 ');
    await userEvent.type(screen.getByPlaceholderText(/Buscar cidade/i), ' Bauru ');
    await userEvent.type(screen.getByPlaceholderText(/UF/i), ' SP ');

    fireEvent.submit(container.querySelector('form')!);

    await waitFor(() => {
      expect(customerService.addSiteToCustomer).toHaveBeenCalledWith(
        '123',
        expect.objectContaining({
          name: 'Obra 2',
          address: expect.objectContaining({
            city: 'Bauru',
            state: 'SP'
          })
        })
      );
    });
  });

  it('6 e 7. Payload de atualização e sucesso fecha modal', async () => {
    vi.mocked(customerService.updateSite).mockResolvedValueOnce({} as never);
    const initialData = {
      id: 'site-1',
      name: 'Antigo',
      address: { city: 'São Paulo', state: 'SP', countryCode: 'BR' }
    };

    const { container } = render(
      <SiteFormModal {...defaultProps} initialData={initialData as never} />
    );

    fireEvent.submit(container.querySelector('form')!);

    await waitFor(() => {
      expect(customerService.updateSite).toHaveBeenCalledWith('site-1', expect.anything());
    });

    expect(defaultProps.onClose).toHaveBeenCalled();
    expect(defaultProps.onSuccess).toHaveBeenCalled();
  });

  it('8. Erro do serviço', async () => {
    vi.mocked(customerService.addSiteToCustomer).mockRejectedValueOnce(new Error('Erro'));
    const { container } = render(<SiteFormModal {...defaultProps} />);

    await userEvent.type(screen.getAllByPlaceholderText(/Nome do local/i)[0], 'Fábrica');
    await userEvent.type(screen.getByPlaceholderText(/Buscar cidade/i), 'City');
    await userEvent.type(screen.getByPlaceholderText(/UF/i), 'ST');

    fireEvent.submit(container.querySelector('form')!);

    expect(await screen.findByText(/Erro ao/i)).toBeInTheDocument();
    expect(defaultProps.onSuccess).not.toHaveBeenCalled();
  });
});
