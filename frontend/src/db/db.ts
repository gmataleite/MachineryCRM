import Dexie, { type Table } from 'dexie';

// Interface do Cliente para armazenamento local
export interface LocalCustomer {
  id: string;
  name: string;
  email?: string;
  phone?: string;
  isSynced: boolean; // Flag para sabermos se veio da API ou se foi criado offline
}

// Interface da Fila de Sincronização (Outbox)
export interface SyncOperation {
  id?: number; // Auto-incrementado localmente
  entity: string; // ex: 'customer'
  action: 'POST' | 'PUT' | 'DELETE';
  payload: any; // Os dados a enviar para a API
  createdAt: number;
}

export class MachineryDatabase extends Dexie {
  customers!: Table<LocalCustomer, string>;
  syncQueue!: Table<SyncOperation, number>;

  constructor() {
    super('MachineryDB');
    
    // Define o esquema das tabelas (apenas a chave primária e campos indexados precisam ser declarados aqui)
    this.version(1).stores({
      customers: 'id, name, isSynced',
      syncQueue: '++id, entity, action, createdAt'
    });
  }
}

export const db = new MachineryDatabase();