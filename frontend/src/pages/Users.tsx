import { useEffect, useMemo, useState } from 'react';
import {
  Users as UsersIcon,
  KeyRound,
  ArrowUp,
  ArrowDown,
  ArrowUpDown,
  Trash2,
  Copy,
  Check,
  X,
  Pencil,
  UserCheck
} from 'lucide-react';
import { Toast, type ToastData } from '../components/Toast';
import { UserFormModal } from '../components/UserFormModal';
import {
  getAppUsers,
  reactivateAppUser,
  resetAppUserPassword,
  deactivateAppUser,
  type AppUserDto
} from '../services/appUserService';

type SortKey = 'fullName' | 'email' | 'role';
type SortDirection = 'asc' | 'desc';

export function Users() {
  const [users, setUsers] = useState<AppUserDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [toast, setToast] = useState<ToastData | null>(null);

  const [filterState, setFilterState] = useState<'open' | 'closed'>('open');
  const [modalConfig, setModalConfig] = useState<{
    isOpen: boolean;
    initialData?: AppUserDto | null;
  }>({ isOpen: false });

  const [sortConfig, setSortConfig] = useState<{ key: SortKey; direction: SortDirection }>({
    key: 'fullName',
    direction: 'asc'
  });

  const [resetModalData, setResetModalData] = useState<{
    isOpen: boolean;
    userName: string;
    tempPass: string;
    copied: boolean;
  }>({
    isOpen: false,
    userName: '',
    tempPass: '',
    copied: false
  });

  const fetchUsersData = async (isInitialRender = false) => {
    try {
      if (!isInitialRender) {
        setLoading(true);
      }
      const data = await getAppUsers();
      setUsers(data);
    } catch (err) {
      console.error('Erro ao buscar usuários', err);
      setToast({ message: 'Falha ao carregar lista de usuários.', type: 'error' });
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    const loadInitialData = async () => {
      await fetchUsersData(true);
    };

    void loadInitialData();
  }, []);

  const handleSort = (key: SortKey) => {
    setSortConfig((prev) => ({
      key,
      direction: prev.key === key && prev.direction === 'asc' ? 'desc' : 'asc'
    }));
  };

  const handleResetPassword = async (user: AppUserDto) => {
    if (!window.confirm(`Gerar nova senha temporária para ${user.fullName}?`)) return;

    try {
      const response = await resetAppUserPassword(user.id);
      setResetModalData({
        isOpen: true,
        userName: user.fullName,
        tempPass: response.temporaryPassword,
        copied: false
      });
      setToast({ message: 'Senha redefinida com sucesso.', type: 'success' });
    } catch (err) {
      console.error('Erro ao redefinir senha', err);
      setToast({ message: 'Falha ao redefinir a senha do usuário.', type: 'error' });
    }
  };

  const handleDeactivate = async (user: AppUserDto) => {
    if (!window.confirm(`Tem certeza que deseja desativar o usuário "${user.fullName}"?`)) return;

    setUsers((prev) => prev.map((u) => (u.id === user.id ? { ...u, isActive: false } : u)));

    try {
      await deactivateAppUser(user.id);
      setToast({ message: 'Usuário desativado com sucesso.', type: 'success' });
    } catch (err) {
      console.error('Erro ao desativar', err);
      setToast({ message: 'Erro ao desativar usuário.', type: 'error' });
      void fetchUsersData();
    }
  };

  const handleReactivate = async (user: AppUserDto) => {
    if (
      !window.confirm(
        `Deseja reativar o usuário "${user.fullName}" e gerar uma nova senha de acesso?`
      )
    )
      return;

    try {
      const response = await reactivateAppUser(user.id);

      // Atualização otimista do estado local para mover o utilizador de volta para a aba "Ativos"
      setUsers((prev) => prev.map((u) => (u.id === user.id ? { ...u, isActive: true } : u)));

      // Reaproveitamos o mesmo Modal da redefinição de senha para exibir a nova senha gerada
      setResetModalData({
        isOpen: true,
        userName: user.fullName,
        tempPass: response.temporaryPassword,
        copied: false
      });
      setToast({ message: 'Usuário reativado com sucesso.', type: 'success' });
    } catch (err) {
      console.error('Erro ao reativar', err);
      setToast({ message: 'Erro ao reativar usuário.', type: 'error' });
      void fetchUsersData(); // reverte em caso de erro
    }
  };

  const handleCopyPass = () => {
    navigator.clipboard.writeText(resetModalData.tempPass);
    setResetModalData((prev) => ({ ...prev, copied: true }));
    setTimeout(() => setResetModalData((prev) => ({ ...prev, copied: false })), 2000);
  };

  const filteredUsers = useMemo(() => {
    const items = users.filter((u) =>
      filterState === 'open' ? u.isActive !== false : u.isActive === false
    );

    items.sort((a, b) => {
      let comparison = 0;
      if (sortConfig.key === 'fullName') {
        comparison = a.fullName.localeCompare(b.fullName, 'pt', { sensitivity: 'base' });
      } else if (sortConfig.key === 'email') {
        comparison = a.email.localeCompare(b.email, 'pt', { sensitivity: 'base' });
      } else if (sortConfig.key === 'role') {
        comparison = a.role.localeCompare(b.role, 'pt', { sensitivity: 'base' });
      }
      return sortConfig.direction === 'asc' ? comparison : -comparison;
    });

    return items;
  }, [users, filterState, sortConfig]);

  const activeCount = users.filter((u) => u.isActive !== false).length;
  const inactiveCount = users.filter((u) => u.isActive === false).length;

  const renderSortIcon = (key: SortKey) => {
    if (sortConfig.key !== key) return <ArrowUpDown size={13} color="#9E9B8F" />;
    return sortConfig.direction === 'asc' ? (
      <ArrowUp size={13} color="#1F3B2C" />
    ) : (
      <ArrowDown size={13} color="#1F3B2C" />
    );
  };

  const sortableHeaderStyle: React.CSSProperties = { cursor: 'pointer', userSelect: 'none' };

  if (loading) return <div className="crm-status">A carregar usuários...</div>;

  return (
    <div>
      <Toast toast={toast} onClose={() => setToast(null)} />

      {modalConfig.isOpen && (
        <UserFormModal
          initialData={modalConfig.initialData}
          onClose={() => setModalConfig({ isOpen: false })}
          onSuccess={(msg) => {
            setToast({ message: msg, type: 'success' });
            void fetchUsersData();
          }}
        />
      )}

      {resetModalData.isOpen && (
        <div className="crm-modal-overlay" style={{ zIndex: 9999 }}>
          <div className="crm-modal" style={{ maxWidth: 450 }}>
            <div className="crm-modal-header">
              <h3 className="crm-modal-title">Senha Redefinida</h3>
              <button
                type="button"
                onClick={() => setResetModalData((p) => ({ ...p, isOpen: false }))}
                className="crm-btn-icon"
              >
                <X size={18} />
              </button>
            </div>
            <p style={{ margin: '0 0 16px', color: '#6E6C61', fontSize: 14 }}>
              A senha temporária para <strong>{resetModalData.userName}</strong> foi gerada. Copie-a
              abaixo:
            </p>
            <div
              style={{
                display: 'flex',
                alignItems: 'center',
                gap: '8px',
                background: '#F2F0E9',
                padding: '12px 14px',
                borderRadius: '4px',
                border: '1px solid #DEDCD0'
              }}
            >
              <strong style={{ flex: 1, letterSpacing: '1px', fontSize: 16 }}>
                {resetModalData.tempPass}
              </strong>
              <button
                type="button"
                onClick={handleCopyPass}
                className="crm-btn-outline"
                style={{ padding: '6px 12px' }}
              >
                {resetModalData.copied ? <Check size={15} color="green" /> : <Copy size={15} />}{' '}
                {resetModalData.copied ? 'Copiado!' : 'Copiar'}
              </button>
            </div>
            <button
              type="button"
              onClick={() => setResetModalData((p) => ({ ...p, isOpen: false }))}
              className="crm-btn-primary"
              style={{ width: '100%', marginTop: 20 }}
            >
              Fechar
            </button>
          </div>
        </div>
      )}

      <div
        className="crm-page-header"
        style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}
      >
        <div style={{ display: 'flex', alignItems: 'center', gap: 10 }}>
          <UsersIcon size={24} color="#1F3B2C" />
          <h1 className="crm-page-title">Gestão de Usuários</h1>
        </div>

        <button
          type="button"
          onClick={() => setModalConfig({ isOpen: true })}
          className="crm-btn-primary"
          style={{ padding: '7px 14px', fontSize: 13 }}
        >
          + Novo usuário
        </button>
      </div>

      <div className="crm-table-wrapper" style={{ marginTop: 24 }}>
        <div style={{ display: 'flex', borderBottom: '1px solid #DEDCD0', background: '#F8F7F2' }}>
          <button
            onClick={() => setFilterState('open')}
            style={{
              display: 'inline-flex',
              alignItems: 'center',
              gap: 6,
              background: 'transparent',
              border: 0,
              padding: '12px 16px',
              fontSize: 14,
              fontWeight: 600,
              cursor: 'pointer',
              color: filterState === 'open' ? '#23291F' : '#6E6C61',
              borderBottom: filterState === 'open' ? '2px solid #1F3B2C' : '2px solid transparent'
            }}
          >
            Ativos{' '}
            <span
              style={{ background: '#E4ECE6', padding: '2px 6px', borderRadius: 12, fontSize: 12 }}
            >
              {activeCount}
            </span>
          </button>

          <button
            onClick={() => setFilterState('closed')}
            style={{
              display: 'inline-flex',
              alignItems: 'center',
              gap: 6,
              background: 'transparent',
              border: 0,
              padding: '12px 16px',
              fontSize: 14,
              fontWeight: 600,
              cursor: 'pointer',
              color: filterState === 'closed' ? '#23291F' : '#6E6C61',
              borderBottom: filterState === 'closed' ? '2px solid #1F3B2C' : '2px solid transparent'
            }}
          >
            Inativos{' '}
            <span
              style={{ background: '#EAE9E4', padding: '2px 6px', borderRadius: 12, fontSize: 12 }}
            >
              {inactiveCount}
            </span>
          </button>
        </div>

        <table className="crm-table">
          <thead>
            <tr>
              <th
                onClick={() => handleSort('fullName')}
                style={{ ...sortableHeaderStyle, width: '35%' }}
              >
                <span style={{ display: 'inline-flex', alignItems: 'center', gap: 6 }}>
                  Nome {renderSortIcon('fullName')}
                </span>
              </th>
              <th onClick={() => handleSort('email')} style={sortableHeaderStyle}>
                <span style={{ display: 'inline-flex', alignItems: 'center', gap: 6 }}>
                  Email {renderSortIcon('email')}
                </span>
              </th>
              <th
                onClick={() => handleSort('role')}
                style={{ ...sortableHeaderStyle, textAlign: 'center' }}
              >
                <span
                  style={{
                    display: 'inline-flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    gap: 6
                  }}
                >
                  Nível de Acesso {renderSortIcon('role')}
                </span>
              </th>
              <th style={{ textAlign: 'right', width: 120 }}>Ações</th>
            </tr>
          </thead>
          <tbody>
            {filteredUsers.length === 0 ? (
              <tr>
                <td colSpan={4} className="crm-table-empty">
                  Nenhum usuário encontrado nesta aba.
                </td>
              </tr>
            ) : (
              filteredUsers.map((user) => (
                <tr key={user.id}>
                  <td style={{ fontWeight: 600, color: '#1F3B2C' }}>{user.fullName}</td>
                  <td style={{ color: '#6E6C61' }}>{user.email}</td>
                  <td style={{ textAlign: 'center' }}>
                    <span
                      style={{
                        background: '#E4ECE6',
                        color: '#2F5240',
                        padding: '3px 8px',
                        borderRadius: 4,
                        fontSize: 12,
                        fontWeight: 600
                      }}
                    >
                      {user.role}
                    </span>
                  </td>
                  <td style={{ textAlign: 'right' }}>
                    <div
                      style={{
                        display: 'flex',
                        alignItems: 'center',
                        justifyContent: 'flex-end',
                        gap: 4
                      }}
                    >
                      {user.isActive ? (
                        <>
                          <button
                            type="button"
                            onClick={() => handleResetPassword(user)}
                            title="Redefinir senha (Gerar temporária)"
                            className="crm-btn-icon"
                            style={{ color: '#9E3F33' }}
                          >
                            <KeyRound size={16} />
                          </button>

                          <button
                            type="button"
                            onClick={() => setModalConfig({ isOpen: true, initialData: user })}
                            title="Editar usuário"
                            className="crm-btn-icon"
                          >
                            <Pencil size={16} />
                          </button>

                          <button
                            type="button"
                            onClick={() => handleDeactivate(user)}
                            title="Desativar usuário"
                            className="crm-btn-icon"
                          >
                            <Trash2 size={16} />
                          </button>
                        </>
                      ) : (
                        <button
                          type="button"
                          onClick={() => handleReactivate(user)}
                          title="Reativar usuário e gerar nova senha"
                          className="crm-btn-icon"
                          style={{ color: '#2F5240' }}
                        >
                          <UserCheck size={16} />
                        </button>
                      )}
                    </div>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
