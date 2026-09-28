import { api } from "./api";

// ============================================================================
// ENUMS & CONSTANTS
// ============================================================================

export const GeoLocationType = {
  Office: 0,
  MachineLocation: 1,
  Waypoint: 2,
} as const;

export type GeoLocationType = (typeof GeoLocationType)[keyof typeof GeoLocationType];

// ============================================================================
// GEOPOINT DTOs
// ============================================================================

export interface GeoPointDto {
  id: string;
  description: string;
  latitude: number;
  longitude: number;
  locationType: GeoLocationType;
  order?: number;
}

export interface CreateGeoPointDto {
  description: string;
  latitude: number;
  longitude: number;
  locationType: GeoLocationType;
  order?: number;
}

export interface UpdateGeoPointDto {
  description: string;
  latitude: number;
  longitude: number;
}

export interface ReorderGeoPointDto {
  id: string;
  order: number;
}

// ============================================================================
// SITE DTOs
// ============================================================================

export interface CreateSiteDto {
  name: string;
  country: string;
  state: string;
  city: string;
  observations?: string;
}

export interface SiteDto extends CreateSiteDto {
  id: string;
  geoPoints?: GeoPointDto[];
}

// ============================================================================
// FISCAL ENTITY DTOs
// ============================================================================

export interface CreateFiscalEntityDto {
  sapPn?: string;
  name: string;
  cnpj?: string;
  cpf?: string;
  country: string;
  state: string;
  city: string;
}

export interface FiscalEntityDto extends CreateFiscalEntityDto {
  id: string;
}

// ============================================================================
// CONTACT DTOs
// ============================================================================

export interface CreateContactDto {
  siteId?: string;
  fiscalEntityId?: string;
  description: string;
  phone?: string;
  email?: string;
}

export interface ContactDto extends CreateContactDto {
  id: string;
}

export interface UpdateContactDto {
  siteId?: string | null;
  fiscalEntityId?: string | null;
  description: string;
  phone?: string;
  email?: string;
}

// ============================================================================
// CUSTOMER DTOs
// ============================================================================

export interface CreateCustomerDto {
  name: string;
}

export interface CustomerDto {
  id: string;
  name: string;
  sites: SiteDto[];
  fiscalEntities: FiscalEntityDto[];
  contacts: ContactDto[];
}

// ============================================================================
// API SERVICES — CUSTOMERS
// ============================================================================

export const getCustomers = async (): Promise<CustomerDto[]> => {
  const response = await api.get<CustomerDto[]>("/customers");
  return response.data;
};

export const getCustomerById = async (id: string): Promise<CustomerDto> => {
  const response = await api.get<CustomerDto>(`/customers/${id}`);
  return response.data;
};

export const createCustomer = async (data: CreateCustomerDto): Promise<CustomerDto> => {
  const response = await api.post<CustomerDto>("/customers", data);
  return response.data;
};

// ============================================================================
// API SERVICES — SITES & FISCAL ENTITIES
// ============================================================================

export const addSiteToCustomer = async (
  customerId: string,
  data: CreateSiteDto
): Promise<SiteDto> => {
  const response = await api.post<SiteDto>(`/customers/${customerId}/sites`, data);
  return response.data;
};

export const addFiscalEntityToCustomer = async (
  customerId: string,
  data: CreateFiscalEntityDto
): Promise<FiscalEntityDto> => {
  const response = await api.post<FiscalEntityDto>(
    `/customers/${customerId}/fiscal-entities`,
    data
  );
  return response.data;
};

// ============================================================================
// API SERVICES — CONTACTS
// ============================================================================

export const addContactToCustomer = async (
  customerId: string,
  data: CreateContactDto
): Promise<ContactDto> => {
  const response = await api.post<ContactDto>(`/customers/${customerId}/contacts`, data);
  return response.data;
};

export const updateContact = async (
  contactId: string,
  data: UpdateContactDto
): Promise<void> => {
  await api.put(`/customers/contacts/${contactId}`, data);
};

// ============================================================================
// API SERVICES — GEOPOINTS
// ============================================================================

export const addGeoPointToSite = async (
  siteId: string,
  data: CreateGeoPointDto
): Promise<GeoPointDto> => {
  const response = await api.post<GeoPointDto>(
    `/customers/sites/${siteId}/geopoints`,
    data
  );
  return response.data;
};

export const updateGeoPoint = async (
  geoPointId: string,
  data: UpdateGeoPointDto
): Promise<void> => {
  await api.put(`/customers/geopoints/${geoPointId}`, data);
};

export const deleteGeoPoint = async (geoPointId: string): Promise<void> => {
  await api.delete(`/customers/geopoints/${geoPointId}`);
};

export const reorderGeoPoints = async (
  siteId: string,
  data: ReorderGeoPointDto[]
): Promise<void> => {
  await api.put(`/customers/sites/${siteId}/geopoints/reorder`, data);
};