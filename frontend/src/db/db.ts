import Dexie, { type Table } from 'dexie';

// Interface do Cliente para armazenamento local
export interface LocalCustomer {
  id: string;
  name: string;
  email?: string;
  phone?: string;
  isSynced: boolean; // Flag para sabermos se veio da API ou se foi criado offline
}

export interface CustomerSyncPayload {
  id: string;
  name: string;
  email?: string;
  phone?: string;
}

type BaseSyncOperation<TEntity extends string, TPayload> =
  | {
      id?: number; // Auto-incrementado localmente
      entity: TEntity;
      action: 'POST' | 'PUT';
      payload: TPayload;
      createdAt: number;
    }
  | {
      id?: number;
      entity: TEntity;
      action: 'DELETE';
      payload: { id: string };
      createdAt: number;
    };

// Interface da Fila de Sincronização (Outbox)
export type SyncOperation = BaseSyncOperation<'customer', CustomerSyncPayload>;

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
