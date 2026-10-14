import { api } from './api';

export interface AppUserDto {
  id: string;
  fullName: string;
  email: string;
  role: string;
  isActive: boolean;
}

export interface CreateAppUserDto {
  fullName: string;
  email: string;
  role: string;
}

export interface UpdateAppUserDto {
  fullName: string;
  email: string;
  role: string;
}

export interface ResetPasswordResultDto {
  temporaryPassword: string;
}

export const getAppUsers = async (): Promise<AppUserDto[]> => {
  const response = await api.get<AppUserDto[]>('/appusers');
  return response.data;
};

export const createAppUser = async (data: CreateAppUserDto): Promise<ResetPasswordResultDto> => {
  const response = await api.post<ResetPasswordResultDto>('/appusers', data);
  return response.data;
};

export const updateAppUser = async (id: string, data: UpdateAppUserDto): Promise<void> => {
  await api.put(`/appusers/${id}`, data);
};

export const deactivateAppUser = async (id: string): Promise<void> => {
  await api.patch(`/appusers/${id}/deactivate`);
};

export const resetAppUserPassword = async (id: string): Promise<ResetPasswordResultDto> => {
  const response = await api.post<ResetPasswordResultDto>(`/appusers/${id}/reset-password`);
  return response.data;
};

export const reactivateAppUser = async (id: string): Promise<ResetPasswordResultDto> => {
  const response = await api.patch<ResetPasswordResultDto>(`/appusers/${id}/reactivate`);
  return response.data;
};