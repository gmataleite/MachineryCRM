import { api } from './api';

export interface MachineDto {
  id: string;
  serialNumber: string;
  model: string;
  year: number;
}

export const getMachines = async (): Promise<MachineDto[]> => {
  const response = await api.get<MachineDto[]>('/machines');
  return response.data;
};