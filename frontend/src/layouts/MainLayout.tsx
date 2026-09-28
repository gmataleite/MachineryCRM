import { Outlet, NavLink } from "react-router-dom";
import { LayoutDashboard, Wrench, Building2 } from "lucide-react";

const navItems = [
  { to: "/", label: "Dashboard", icon: LayoutDashboard, end: true },
  { to: "/machines", label: "Equipamentos", icon: Wrench, end: false },
  { to: "/customers", label: "Clientes", icon: Building2, end: false },
];

export function MainLayout() {
  return (
    <div style={{ display: "flex", minHeight: "100vh", width: "100%" }}>
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
        style={{
          flex: 1,
          minWidth: 0,
          background: "#F2F0E9",
          color: "#23291F",
          minHeight: "100vh",
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