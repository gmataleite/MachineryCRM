import { useEffect, useState } from "react";
import { LayoutDashboard, Wrench, Building2, UserPlus, KeyRound, LogOut, Pin, PinOff } from "lucide-react";
import { NavLink, Outlet, useNavigate } from "react-router-dom";
import { useAuth } from "../contexts/AuthContext";
const navItems = [
  { to: "/", label: "Dashboard", icon: LayoutDashboard, end: true },
  { to: "/machines", label: "Equipamentos", icon: Wrench, end: false },
  { to: "/customers", label: "Clientes", icon: Building2, end: false },
];

export function MainLayout() {
  const { role, logout } = useAuth();
  const navigate = useNavigate();
  const canManageUsers = role === "Admin" || role === "Manager";
  const [isMobile, setIsMobile] = useState(() => window.matchMedia("(max-width: 700px)").matches);
  const [isPinned, setIsPinned] = useState(() => localStorage.getItem("sidebar_pinned") === "true");
  const [isHovered, setIsHovered] = useState(false);
  const [isMobileOpen, setIsMobileOpen] = useState(false);

  useEffect(() => {
    const mediaQuery = window.matchMedia("(max-width: 700px)");
    const handleChange = () => {
      setIsMobile(mediaQuery.matches);
      setIsMobileOpen(false);
      setIsHovered(false);
    };

    handleChange();
    mediaQuery.addEventListener("change", handleChange);
    return () => mediaQuery.removeEventListener("change", handleChange);
  }, []);

  const isExpanded = isMobile ? isMobileOpen : isPinned || isHovered;

  function togglePinned() {
    const nextPinned = !isPinned;
    setIsPinned(nextPinned);
    localStorage.setItem("sidebar_pinned", String(nextPinned));
    if (isMobile) setIsMobileOpen(nextPinned);
  }

  return (
    <div style={{ display: "flex", minHeight: "100vh", width: "100%" }}>
      <aside
        onMouseEnter={() => !isMobile && setIsHovered(true)}
        onMouseLeave={() => !isMobile && setIsHovered(false)}
        style={{
          width: isExpanded ? 240 : 64,
          flexShrink: 0,
          backgroundColor: "#1F3B2C",
          color: "#FFFFFF",
          padding: "16px 10px",
          display: "flex",
          flexDirection: "column",
          transition: "width 0.2s ease",
          overflow: "hidden",
          zIndex: 10,
        }}
      >
        <div
          style={{
            display: "flex",
            alignItems: "center",
            justifyContent: isExpanded ? "space-between" : "center",
            minHeight: 36,
            marginBottom: 20,
          }}
        >
          {isExpanded && (
            <div style={{ fontSize: 19, fontWeight: 700, padding: "0 4px", letterSpacing: "-0.01em", color: "#FFFFFF", whiteSpace: "nowrap" }}>
              MachineryCRM
            </div>
          )}
          <button
            type="button"
            onClick={togglePinned}
            title={isPinned ? "Desafixar menu" : "Fixar menu"}
            aria-label={isPinned ? "Desafixar menu" : "Fixar menu"}
            style={{ display: "inline-flex", alignItems: "center", justifyContent: "center", width: 34, height: 34, color: "#FFFFFF", background: "rgba(255,255,255,.1)", border: 0, borderRadius: 4, cursor: "pointer" }}
          >
            {isPinned ? <PinOff size={17} /> : <Pin size={17} />}
          </button>
        </div>

        <nav style={{ display: "flex", flexDirection: "column", gap: 6 }}>
          {[...navItems, ...(canManageUsers ? [{ to: "/users/new", label: "Novo usuário", icon: UserPlus, end: false }] : [])].map(({ to, label, icon: Icon, end }) => (
            <NavLink
              key={to}
              to={to}
              end={end}
              style={({ isActive }) => ({
                display: "flex",
                alignItems: "center",
                justifyContent: isExpanded ? "flex-start" : "center",
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
              {isExpanded && <span style={{ whiteSpace: "nowrap" }}>{label}</span>}
            </NavLink>
          ))}
        </nav>
        <div style={{ marginTop: "auto", display: "flex", flexDirection: "column", gap: 6 }}>
          <NavLink to="/change-password" title="Alterar senha" style={{ display: "flex", alignItems: "center", justifyContent: isExpanded ? "flex-start" : "center", gap: 10, padding: "9px 12px", color: "#DEDCD0", textDecoration: "none", fontSize: 14 }}>
            <KeyRound size={17} /> {isExpanded && <span>Alterar senha</span>}
          </NavLink>
          <button title="Sair" onClick={() => { logout(); navigate("/login"); }} style={{ display: "flex", alignItems: "center", justifyContent: isExpanded ? "flex-start" : "center", gap: 10, padding: "9px 12px", color: "#DEDCD0", background: "transparent", border: 0, fontSize: 14, cursor: "pointer", fontFamily: "inherit" }}>
            <LogOut size={17} /> {isExpanded && <span>Sair</span>}
          </button>
        </div>
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