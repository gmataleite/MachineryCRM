import { useEffect } from 'react';
import { db } from '../db/db';
import { api } from '../services/api';

export function useBackgroundSync() {
  useEffect(() => {
    const processQueue = async () => {
      if (!navigator.onLine) return;

      const pendingOperations = await db.syncQueue.orderBy('createdAt').toArray();
      if (pendingOperations.length === 0) return;

      console.log(`Processando ${pendingOperations.length} operações pendentes...`);

      for (const op of pendingOperations) {
        try {
          if (op.action === 'POST' && op.entity === 'customer') {
            await api.post('/customers', op.payload);

            // Marca o cliente local como sincronizado
            await db.customers.update(op.payload.id, { isSynced: true });
          }
          // (Aqui você adicionaria os ifs para PUT e DELETE)

          // Se a requisição deu sucesso (HTTP 200/201), remove da fila local
          await db.syncQueue.delete(op.id!);
        } catch (error) {
          console.error(
            'Erro ao sincronizar operação, ficará na fila para a próxima tentativa.',
            error
          );
          break; // Para a fila para respeitar a ordem cronológica em caso de erro na API
        }
      }
    };

    // Escuta ativamente quando a rede do dispositivo volta a ficar online
    window.addEventListener('online', processQueue);

    // Tenta processar assim que o app é aberto
    void processQueue();

    return () => {
      window.removeEventListener('online', processQueue);
    };
  }, []);
}
