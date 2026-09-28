import { Outlet, NavLink } from "react-router-dom";
import { LayoutDashboard, Wrench, Building2 } from "lucide-react";

const navItems = [
  { to: "/", label: "Dashboard", icon: LayoutDashboard, end: true },
  { to: "/machines", label: "Equipamentos", icon: Wrench, end: false },
  { to: "/customers", label: "Clientes", icon: Building2, end: false },
];

export function MainLayout() {
  return (
    <div
      style={{
        display: "flex",
        minHeight: "100vh",
        fontFamily: "'IBM Plex Sans', sans-serif",
        background: "#F2F0E9",
        color: "#23291F",
      }}
    >
      <style>{`
        .crm-main * {
          box-sizing: border-box;
        }
        .crm-page-header {
          display: flex;
          align-items: center;
          gap: 8px;
          margin-bottom: 20px;
        }
        .crm-page-title {
          font-size: 22px;
          font-weight: 700;
          margin: 0;
          color: #23291F;
        }
        .crm-section-header {
          display: flex;
          justify-content: space-between;
          align-items: center;
          margin-bottom: 14px;
        }
        .crm-section-title {
          font-size: 18px;
          font-weight: 700;
          margin: 0;
          color: #23291F;
        }
        .crm-card-accent {
          background: #FFFFFF;
          padding: 22px 24px;
          margin-bottom: 24px;
          border-left: 4px solid #1F3B2C;
          border-radius: 4px;
          box-shadow: 0 1px 3px rgba(0, 0, 0, 0.04);
        }
        .crm-card {
          background: #FFFFFF;
          padding: 20px 24px;
          margin-bottom: 16px;
          border-radius: 4px;
          border: 1px solid #EAE8DD;
          box-shadow: 0 1px 3px rgba(0, 0, 0, 0.03);
        }
        .crm-card-divider {
          margin-top: 16px;
          border-top: 1px solid #EAE8DD;
          padding-top: 14px;
        }
        .crm-label {
          display: block;
          font-size: 12px;
          font-weight: 600;
          color: #6E6C61;
          margin-bottom: 6px;
        }
        .crm-input {
          padding: 9px 12px;
          background: #F2F0E9;
          border: 1px solid #DEDCD0;
          border-radius: 4px;
          font-size: 14px;
          color: #23291F;
          font-family: inherit;
          outline: none;
        }
        .crm-input:focus {
          border-color: #1F3B2C;
        }
        .crm-btn-primary {
          background: #1F3B2C;
          color: #FFFFFF;
          border: none;
          padding: 8px 16px;
          border-radius: 4px;
          cursor: pointer;
          font-size: 13.5px;
          font-weight: 600;
          font-family: inherit;
          display: inline-flex;
          align-items: center;
          justify-content: center;
          gap: 6px;
        }
        .crm-btn-outline {
          background: #FFFFFF;
          border: 1px solid #DEDCD0;
          color: #23291F;
          padding: 5px 12px;
          border-radius: 4px;
          cursor: pointer;
          font-size: 12.5px;
          font-weight: 600;
          font-family: inherit;
          display: inline-flex;
          align-items: center;
          gap: 4px;
        }
        .crm-btn-icon {
          background: none;
          border: none;
          cursor: pointer;
          color: #6E6C61;
          padding: 4px;
          display: inline-flex;
          align-items: center;
          justify-content: center;
          border-radius: 4px;
        }
        .crm-btn-back {
          background: none;
          border: none;
          cursor: pointer;
          display: inline-flex;
          align-items: center;
          gap: 6px;
          color: #6E6C61;
          margin-bottom: 18px;
          padding: 0;
          font-size: 13.5px;
          font-family: inherit;
        }
        .crm-table-wrapper {
          background: #FFFFFF;
          border-radius: 4px;
          overflow-x: auto;
          border: 1px solid #EAE8DD;
          box-shadow: 0 1px 3px rgba(0, 0, 0, 0.04);
        }
        .crm-table {
          width: 100%;
          border-collapse: collapse;
          text-align: left;
          font-size: 14px;
        }
        .crm-table thead tr {
          background: #F8F7F2;
          border-bottom: 2px solid #DEDCD0;
        }
        .crm-table th {
          padding: 12px 16px;
          color: #6E6C61;
          font-weight: 600;
          font-size: 13px;
        }
        .crm-table tbody tr {
          border-bottom: 1px solid #EAE8DD;
        }
        .crm-table tbody tr:last-child {
          border-bottom: none;
        }
        .crm-table td {
          padding: 12px 16px;
          color: #23291F;
        }
        .crm-table-empty {
          padding: 20px 16px;
          text-align: center;
          color: #6E6C61;
          font-style: italic;
        }
        .crm-mono-id {
          font-family: monospace;
          font-size: 12px;
          color: #8C897E;
        }
        .crm-empty-hint {
          font-style: italic;
          color: #8C897E;
          font-size: 13px;
          padding: 4px 0;
        }
        .crm-status {
          font-size: 14px;
          color: #6E6C61;
          padding: 4px 0;
        }
        .crm-error {
          background: #F5E6E4;
          color: #9E3F33;
          border: 1px solid #E5C5C0;
          padding: 10px 14px;
          border-radius: 4px;
          font-size: 13.5px;
          margin-bottom: 16px;
        }
      `}</style>

      <aside
        style={{
          width: 240,
          flexShrink: 0,
          backgroundColor: "#1F3B2C",
          color: "#FFFFFF",
          padding: "24px 16px",
          display: "flex",
          flexDirection: "column",
        }}
      >
        <div
          style={{
            fontSize: 19,
            fontWeight: 700,
            marginBottom: 28,
            padding: "0 12px",
            letterSpacing: "-0.01em",
            color: "#FFFFFF",
          }}
        >
          MachineryCRM
        </div>

        <nav style={{ display: "flex", flexDirection: "column", gap: 6 }}>
          {navItems.map(({ to, label, icon: Icon, end }) => (
            <NavLink
              key={to}
              to={to}
              end={end}
              style={({ isActive }) => ({
                display: "flex",
                alignItems: "center",
                gap: 10,
                padding: "9px 12px",
                borderRadius: 4,
                textDecoration: "none",
                fontSize: 14,
                fontWeight: isActive ? 600 : 500,
                color: isActive ? "#FFFFFF" : "#DEDCD0",
                background: isActive ? "rgba(255, 255, 255, 0.1)" : "transparent",
                transition: "background 0.15s ease, color 0.15s ease",
              })}
            >
              <Icon size={17} />
              {label}
            </NavLink>
          ))}
        </nav>
      </aside>

      <main
        className="crm-main"
        style={{
          flex: 1,
          minWidth: 0,
          background: "#F2F0E9",
          color: "#23291F",
          minHeight: "100vh",
          boxSizing: "border-box",
          textAlign: "left",
        }}
      >
        <div style={{ maxWidth: 1000, margin: "0 auto", padding: "30px 28px 80px" }}>
          <Outlet />
        </div>
      </main>
    </div>
  );
}