import { api } from './api';
import { db, type LocalCustomer } from '../db/db';
import { v4 as uuidv4 } from 'uuid';

export const saveCustomer = async (customerData: Omit<LocalCustomer, 'id' | 'isSynced'>) => {
  const newId = uuidv4(); // Geramos o ID no frontend para garantir que o BD local tenha a chave
  const payload = { id: newId, ...customerData };

  if (navigator.onLine) {
    try {
      // 1. Tenta salvar na API .NET
      await api.post('/customers', payload);

      // 2. Se deu certo, salva no banco local como sincronizado
      await db.customers.put({ ...payload, isSynced: true });
      return;
    } catch (error) {
      console.warn('Falha na API, salvando offline como fallback...', error);
      // Se a API cair (500) mesmo com internet, tratamos como offline
    }
  }

  // 3. FLUXO OFFLINE: Salva no BD local como pendente
  await db.transaction('rw', db.customers, db.syncQueue, async () => {
    await db.customers.put({ ...payload, isSynced: false });

    // Adiciona a operação à fila para ser processada mais tarde
    await db.syncQueue.add({
      entity: 'customer',
      action: 'POST',
      payload: payload,
      createdAt: Date.now()
    });
  });
};

export const getLocalCustomers = async () => {
  // Retorna instantaneamente os dados do celular do usuário
  return await db.customers.toArray();
};
