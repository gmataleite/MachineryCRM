import { LayoutDashboard } from "lucide-react";

export function Dashboard() {
  return (
    <div>
      <div className="crm-page-header">
        <LayoutDashboard size={24} color="#1F3B2C" />
        <h1 className="crm-page-title">Dashboard</h1>
      </div>

      <div className="crm-card-accent">
        <h2
          style={{
            fontSize: 13,
            fontWeight: 700,
            letterSpacing: "0.04em",
            marginTop: 0,
            marginBottom: 8,
            color: "#6E6C61",
          }}
        >
          VISÃO GERAL DO SISTEMA
        </h2>
        <p style={{ margin: 0, fontSize: 14, color: "#23291F" }}>
          Visão geral do sistema (Métricas e Ordens de Manutenção).
        </p>
      </div>
    </div>
  );
}