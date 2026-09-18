import { api } from './api';

export interface SiteDto {
  id: string;
  name: string;
  country: string;
  state: string;
  city: string;
  observations?: string;
}

export interface FiscalEntityDto {
  id: string;
  sapPn?: string;
  name: string;
  cnpj?: string;
  cpf?: string;
  country: string;
  state: string;
  city: string;
}

export interface ContactDto {
  id: string;
  siteId?: string;
  fiscalEntityId?: string;
  description: string;
  phone?: string;
  email?: string;
}

export interface CustomerDto {
  id: string;
  name: string;
  sites: SiteDto[];
  fiscalEntities: FiscalEntityDto[];
  contacts: ContactDto[];
}

export interface CreateCustomerDto {
  name: string;
}

export interface CreateSiteDto {
  name: string;
  country: string;
  state: string;
  city: string;
  observations?: string;
}

export interface CreateFiscalEntityDto {
  sapPn?: string;
  name: string;
  cnpj?: string;
  cpf?: string;
  country: string;
  state: string;
  city: string;
}

export const getCustomers = async (): Promise<CustomerDto[]> => {
  const response = await api.get<CustomerDto[]>('/customers');
  return response.data;
};

export const getCustomerById = async (id: string): Promise<CustomerDto> => {
  const response = await api.get<CustomerDto>(`/customers/${id}`);
  return response.data;
};

export const createCustomer = async (data: CreateCustomerDto): Promise<CustomerDto> => {
  const response = await api.post<CustomerDto>('/customers', data);
  return response.data;
};

export const addSiteToCustomer = async (customerId: string, data: CreateSiteDto): Promise<SiteDto> => {
  const response = await api.post<SiteDto>(`/customers/${customerId}/sites`, data);
  return response.data;
};

export const addFiscalEntityToCustomer = async (customerId: string, data: CreateFiscalEntityDto): Promise<FiscalEntityDto> => {
  const response = await api.post<FiscalEntityDto>(`/customers/${customerId}/fiscal-entities`, data);
  return response.data;
};