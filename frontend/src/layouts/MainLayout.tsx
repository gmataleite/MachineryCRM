import { Outlet, Link } from 'react-router-dom';

export function MainLayout() {
  return (
    <div style={{ display: 'flex', minHeight: '100vh', fontFamily: 'sans-serif' }}>
      <aside style={{ width: '250px', backgroundColor: '#1e293b', color: '#fff', padding: '1rem' }}>
        <h2 style={{ fontSize: '1.5rem', marginBottom: '2rem' }}>MachineryCRM</h2>
        <nav style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
          <Link to="/" style={{ color: '#cbd5e1', textDecoration: 'none' }}>Dashboard</Link>
          <Link to="/machines" style={{ color: '#cbd5e1', textDecoration: 'none' }}>Equipamentos</Link>
        </nav>
      </aside>
      <main style={{ flex: 1, padding: '2rem', backgroundColor: '#f8fafc' }}>
        <Outlet />
      </main>
    </div>
  );
}