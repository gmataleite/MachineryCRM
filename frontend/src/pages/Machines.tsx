import { useEffect, useState } from 'react';
import { getMachines, type MachineDto } from '../services/machineService';

export function Machines() {
  const [machines, setMachines] = useState<MachineDto[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchMachines = async () => {
      try {
        const data = await getMachines();
        setMachines(data);
      } catch (err) {
        setError('Falha ao carregar os dados dos equipamentos.');
      } finally {
        setLoading(false);
      }
    };

    fetchMachines();
  }, []);

  if (loading) return <div>Carregando equipamentos...</div>;
  if (error) return <div style={{ color: 'red', padding: '1rem' }}>{error}</div>;

  return (
    <div>
      <h1 style={{ fontSize: '1.5rem', marginBottom: '1.5rem' }}>Gestão de Equipamentos</h1>
      
      <div style={{ overflowX: 'auto' }}>
        <table style={{ width: '100%', borderCollapse: 'collapse', backgroundColor: '#fff', boxShadow: '0 1px 3px rgba(0,0,0,0.1)', borderRadius: '8px' }}>
          <thead>
            <tr style={{ backgroundColor: '#f1f5f9', borderBottom: '2px solid #cbd5e1', textAlign: 'left' }}>
              <th style={{ padding: '1rem' }}>Número de Série</th>
              <th style={{ padding: '1rem' }}>Modelo</th>
              <th style={{ padding: '1rem' }}>Ano</th>
            </tr>
          </thead>
          <tbody>
            {machines.length === 0 ? (
              <tr>
                <td colSpan={3} style={{ padding: '1.5rem', textAlign: 'center', color: '#64748b' }}>
                  Nenhum equipamento registrado.
                </td>
              </tr>
            ) : (
              machines.map((machine) => (
                <tr key={machine.id} style={{ borderBottom: '1px solid #e2e8f0' }}>
                  <td style={{ padding: '1rem' }}>{machine.serialNumber}</td>
                  <td style={{ padding: '1rem' }}>{machine.model}</td>
                  <td style={{ padding: '1rem' }}>{machine.year}</td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}