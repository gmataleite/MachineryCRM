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
  order?: number;
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
  address?: AddressDto | null;
  observations?: string;
}

export interface SiteDto extends CreateSiteDto {
  id: string;
  geoPoints?: GeoPointDto[];
}

export interface UpdateSiteDto extends CreateSiteDto { }

// ============================================================================
// FISCAL ENTITY DTOs
// ============================================================================

export interface AddressDto {
  addressLine?: string | null;
  neighborhood?: string | null;
  city?: string | null;
  state?: string | null;
  postalCode?: string | null;
  countryCode?: string | null;
}

export interface TaxIdDto {
  value: string;
}

export interface CreateFiscalEntityDto {
  name: string;
  taxId?: TaxIdDto | null;
  billingAddress?: AddressDto | null;
  shippingAddress?: AddressDto | null;
}

export interface FiscalEntityDto extends CreateFiscalEntityDto {
  id: string;
}

export interface UpdateFiscalEntityDto extends CreateFiscalEntityDto { }

// ============================================================================
// CONTACT DTOs
// ============================================================================

export interface CreateContactDto {
  name: string;
  phone?: string;
  email?: string;
  observations?: string;
  siteId?: string | null;
  fiscalEntityId?: string | null;
}

export interface ContactDto extends CreateContactDto {
  id: string;
}

export interface UpdateContactDto extends CreateContactDto { }

// ============================================================================
// CUSTOMER DTOs
// ============================================================================

export interface CreateCustomerDto {
  name: string;
}

export interface CustomerDto {
  id: string;
  name: string;
  isActive: boolean;
  sites: SiteDto[];
  fiscalEntities: FiscalEntityDto[];
  contacts: ContactDto[];
}

export interface UpdateCustomerDto extends CreateCustomerDto { }

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

export const updateCustomer = async (id: string, data: UpdateCustomerDto): Promise<void> => {
  await api.put(`/customers/${id}`, data);
};

export const deactivateCustomer = async (id: string): Promise<void> => {
  await api.patch(`/customers/${id}/deactivate`);
};

// ============================================================================
// API SERVICES — SITES
// ============================================================================

export const addSiteToCustomer = async (
  customerId: string,
  data: CreateSiteDto
): Promise<SiteDto> => {
  const response = await api.post<SiteDto>(`/customers/${customerId}/sites`, data);
  return response.data;
};

export const updateSite = async (siteId: string, data: UpdateSiteDto): Promise<void> => {
  await api.put(`/customers/sites/${siteId}`, data);
};

export const deleteSite = async (siteId: string): Promise<void> => {
  await api.delete(`/customers/sites/${siteId}`);
};

// ============================================================================
// API SERVICES — FISCAL ENTITIES
// ============================================================================

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

export const updateFiscalEntity = async (fiscalId: string, data: UpdateFiscalEntityDto): Promise<void> => {
  await api.put(`/customers/fiscal-entities/${fiscalId}`, data);
};

export const deleteFiscalEntity = async (fiscalId: string): Promise<void> => {
  await api.delete(`/customers/fiscal-entities/${fiscalId}`);
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

export const deleteContact = async (contactId: string): Promise<void> => {
  await api.delete(`/customers/contacts/${contactId}`);
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