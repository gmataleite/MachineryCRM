import { useEffect, useState } from "react";
import { Wrench } from "lucide-react";
import { getMachines, type MachineDto } from "../services/machineService";

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
        setError("Falha ao carregar os dados dos equipamentos.");
      } finally {
        setLoading(false);
      }
    };

    fetchMachines();
  }, []);

  if (loading) return <div className="crm-status">A carregar equipamentos...</div>;
  if (error) return <div className="crm-error">{error}</div>;

  return (
    <div>
      <div className="crm-page-header">
        <Wrench size={24} color="#1F3B2C" />
        <h1 className="crm-page-title">Gestão de Equipamentos</h1>
      </div>

      <div className="crm-table-wrapper">
        <table className="crm-table">
          <thead>
            <tr>
              <th>Número de Série</th>
              <th>Modelo</th>
              <th style={{ textAlign: "center" }}>Ano</th>
            </tr>
          </thead>
          <tbody>
            {machines.length === 0 ? (
              <tr>
                <td colSpan={3} className="crm-table-empty">
                  Nenhum equipamento registrado.
                </td>
              </tr>
            ) : (
              machines.map((machine) => (
                <tr key={machine.id}>
                  <td className="crm-mono-id" style={{ fontSize: 13, color: "#23291F", fontWeight: 600 }}>
                    {machine.serialNumber}
                  </td>
                  <td>{machine.model}</td>
                  <td style={{ textAlign: "center" }}>{machine.year}</td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}