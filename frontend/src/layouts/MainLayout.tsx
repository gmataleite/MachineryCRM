import { useEffect, useState } from 'react';
import {
  LayoutDashboard,
  Wrench,
  Building2,
  UserPlus,
  KeyRound,
  LogOut,
  Menu,
  X
} from 'lucide-react';
import { NavLink, Link, Outlet, useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

const navItems = [
  { to: '/', label: 'Dashboard', icon: LayoutDashboard, end: true },
  { to: '/machines', label: 'Equipamentos', icon: Wrench, end: false },
  { to: '/customers', label: 'Clientes', icon: Building2, end: false }
];

export function MainLayout() {
  const { role, logout } = useAuth();
  const navigate = useNavigate();
  const canManageUsers = role === 'Admin' || role === 'Manager';

  const [isMobile, setIsMobile] = useState(() => window.matchMedia('(max-width: 700px)').matches);
  const [isOpen, setIsOpen] = useState(false);

  useEffect(() => {
    const mediaQuery = window.matchMedia('(max-width: 700px)');
    const handleChange = () => {
      setIsMobile(mediaQuery.matches);
      if (mediaQuery.matches) {
        setIsOpen(false);
      }
    };

    handleChange();
    mediaQuery.addEventListener('change', handleChange);
    return () => mediaQuery.removeEventListener('change', handleChange);
  }, []);

  const handleNavClick = () => {
    if (isMobile) setIsOpen(false);
  };

  return (
    <div
      style={{
        display: 'flex',
        height: '100vh',
        width: '100%',
        position: 'relative',
        overflow: 'hidden'
      }}
    >
      {/* Overlay escuro para quando o menu está aberto em ecrãs móveis */}
      {isMobile && isOpen && (
        <div
          style={{
            position: 'fixed',
            inset: 0,
            backgroundColor: 'rgba(0,0,0,0.5)',
            zIndex: 40
          }}
          onClick={() => setIsOpen(false)}
        />
      )}

      {/* Barra Lateral (Sidebar) */}
      <aside
        style={{
          position: isMobile ? 'fixed' : 'relative',
          top: 0,
          left: 0,
          height: '100vh',
          width: isOpen ? 240 : 0,
          flexShrink: 0,
          backgroundColor: '#1F3B2C',
          color: '#FFFFFF',
          transition: 'width 0.3s cubic-bezier(0.4, 0, 0.2, 1)',
          overflowX: 'hidden',
          overflowY: 'auto', // Permite rolar o menu se houver muitos itens
          zIndex: 50,
          boxShadow: isOpen && isMobile ? '4px 0 16px rgba(0,0,0,0.2)' : 'none'
        }}
      >
        <div
          style={{
            width: 240,
            height: '100%',
            display: 'flex',
            flexDirection: 'column',
            padding: '16px 10px'
          }}
        >
          <div
            style={{
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'space-between',
              minHeight: 36,
              marginBottom: 20
            }}
          >
            <div
              style={{
                fontSize: 19,
                fontWeight: 700,
                padding: '0 4px',
                letterSpacing: '-0.01em',
                color: '#FFFFFF',
                whiteSpace: 'nowrap'
              }}
            >
            {import.meta.env.VITE_APP_SHORT_NAME || 'MachineryCRM'}
            </div>
            <button
              type="button"
              onClick={() => setIsOpen(false)}
              title="Fechar menu"
              style={{
                display: 'inline-flex',
                alignItems: 'center',
                justifyContent: 'center',
                width: 34,
                height: 34,
                color: '#FFFFFF',
                background: 'rgba(255,255,255,.1)',
                border: 0,
                borderRadius: 4,
                cursor: 'pointer'
              }}
            >
              <X size={17} />
            </button>
          </div>

          <nav style={{ display: 'flex', flexDirection: 'column', gap: 6 }}>
            {[
              ...navItems,
              ...(canManageUsers
                ? [{ to: '/users', label: 'Usuários', icon: UserPlus, end: false }]
                : [])
            ].map(({ to, label, icon: Icon, end }) => (
              <NavLink
                key={to}
                to={to}
                end={end}
                onClick={handleNavClick}
                style={({ isActive }) => ({
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'flex-start',
                  gap: 10,
                  padding: '9px 12px',
                  borderRadius: 4,
                  textDecoration: 'none',
                  fontSize: 14,
                  fontWeight: isActive ? 600 : 500,
                  color: isActive ? '#FFFFFF' : '#DEDCD0',
                  background: isActive ? 'rgba(255, 255, 255, 0.1)' : 'transparent',
                  transition: 'background 0.15s ease, color 0.15s ease'
                })}
              >
                <Icon size={17} />
                <span style={{ whiteSpace: 'nowrap' }}>{label}</span>
              </NavLink>
            ))}
          </nav>

          <div style={{ marginTop: 'auto', display: 'flex', flexDirection: 'column', gap: 6 }}>
            <NavLink
              to="/change-password"
              title="Alterar senha"
              onClick={handleNavClick}
              style={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'flex-start',
                gap: 10,
                padding: '9px 12px',
                color: '#DEDCD0',
                textDecoration: 'none',
                fontSize: 14
              }}
            >
              <KeyRound size={17} /> <span>Alterar senha</span>
            </NavLink>
            <button
              title="Sair"
              onClick={() => {
                logout();
                navigate('/login');
              }}
              style={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'flex-start',
                gap: 10,
                padding: '9px 12px',
                color: '#DEDCD0',
                background: 'transparent',
                border: 0,
                fontSize: 14,
                cursor: 'pointer',
                fontFamily: 'inherit'
              }}
            >
              <LogOut size={17} /> <span>Sair</span>
            </button>
          </div>
        </div>
      </aside>

      {/* Área Principal de Conteúdo */}
      <main
        style={{
          flex: 1,
          minWidth: 0,
          background: '#F2F0E9',
          color: '#23291F',
          height: '100vh',
          overflowY: 'auto', // Permite que apenas a área principal role
          display: 'flex',
          flexDirection: 'column',
          textAlign: 'left'
        }}
      >
        {/* Barra Superior (Header) */}
        <header
          style={{
            height: 64,
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'flex-start',
            gap: 16,
            padding: '0 28px',
            flexShrink: 0
          }}
        >
          <button
            type="button"
            onClick={() => setIsOpen(!isOpen)}
            title="Menu"
            style={{
              background: 'transparent',
              border: 0,
              cursor: 'pointer',
              color: '#1F3B2C',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              padding: '4px'
            }}
          >
            <Menu size={26} />
          </button>

          <Link
            to="/"
            style={{
              fontSize: 18,
              fontWeight: 700,
              color: '#1F3B2C',
              opacity: isOpen ? 0 : 1,
              transition: 'opacity 0.2s',
              textDecoration: 'none'
            }}
          >
            {!isOpen && (import.meta.env.VITE_APP_NAME || 'MachineryCRM')}
          </Link>
        </header>

        {/* Rotas renderizadas do React Router */}
        <div style={{ maxWidth: 1000, margin: '0 auto', padding: '10px 28px 80px', width: '100%' }}>
          <Outlet />
        </div>
      </main>
    </div>
  );
}
