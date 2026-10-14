import React, { useState } from 'react';
import { X, Copy, Check } from 'lucide-react';
import { createAppUser, updateAppUser, type AppUserDto } from '../services/appUserService';

const roles = [
  { value: 'Admin', label: 'Administrador' },
  { value: 'Manager', label: 'Gerente' },
  { value: 'Sales', label: 'Vendas' },
  { value: 'Technician', label: 'Técnico' }
];

interface UserFormModalProps {
  initialData?: AppUserDto | null;
  onClose: () => void;
  onSuccess: (msg: string) => void;
}

export function UserFormModal({ initialData, onClose, onSuccess }: UserFormModalProps) {
  const [fullName, setFullName] = useState(initialData?.fullName || '');
  const [email, setEmail] = useState(initialData?.email || '');
  const [role, setRole] = useState(initialData?.role || 'Technician');

  const [isSubmitting, setIsSubmitting] = useState(false);
  const [errorMsg, setErrorMsg] = useState('');

  const [temporaryPassword, setTemporaryPassword] = useState('');
  const [copied, setCopied] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErrorMsg('');
    setIsSubmitting(true);

    try {
      const payload = { fullName, email, role };

      if (initialData) {
        await updateAppUser(initialData.id, payload);
        onSuccess('Usuário atualizado com sucesso.');
        onClose();
      } else {
        const response = await createAppUser(payload);
        setTemporaryPassword(response.temporaryPassword);
        onSuccess('Usuário cadastrado com sucesso.');
      }
    } catch (err) {
      const error = err as { response?: { status: number } };

      if (error.response?.status === 409) {
        setErrorMsg('Este email já está em uso por outro usuário.');
      } else {
        setErrorMsg(initialData ? 'Erro ao atualizar usuário.' : 'Erro ao cadastrar usuário.');
      }
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleCopy = () => {
    navigator.clipboard.writeText(temporaryPassword);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  };

  return (
    <div className="crm-modal-overlay" style={{ zIndex: 9999 }}>
      <div className="crm-modal">
        <div className="crm-modal-header">
          <h3 className="crm-modal-title">{initialData ? 'Editar Usuário' : 'Novo Usuário'}</h3>
          <button type="button" onClick={onClose} className="crm-btn-icon">
            <X size={18} />
          </button>
        </div>

        {temporaryPassword ? (
          <div>
            <div
              style={{
                padding: '16px',
                background: '#E6F0E8',
                borderLeft: '4px solid #1F3B2C',
                borderRadius: '4px',
                marginBottom: '20px'
              }}
            >
              <p style={{ margin: '0 0 8px', color: '#1F3B2C', fontWeight: 600 }}>
                Senha Temporária Gerada
              </p>
              <p style={{ margin: '0 0 12px', fontSize: '13px', color: '#2F5240' }}>
                Entregue esta senha ao usuário. Ele deverá alterá-la no primeiro acesso.
              </p>
              <div
                style={{
                  display: 'flex',
                  alignItems: 'center',
                  gap: '8px',
                  background: '#FFFFFF',
                  padding: '10px 14px',
                  borderRadius: '4px',
                  border: '1px solid #B8D0BC'
                }}
              >
                <strong style={{ flex: 1, letterSpacing: '1px' }}>{temporaryPassword}</strong>
                <button
                  type="button"
                  onClick={handleCopy}
                  className="crm-btn-outline"
                  style={{ padding: '4px 8px', fontSize: '12px' }}
                >
                  {copied ? <Check size={14} color="green" /> : <Copy size={14} />}{' '}
                  {copied ? 'Copiado!' : 'Copiar'}
                </button>
              </div>
            </div>
            <button
              type="button"
              onClick={onClose}
              className="crm-btn-primary"
              style={{ width: '100%' }}
            >
              Concluir
            </button>
          </div>
        ) : (
          <form onSubmit={handleSubmit} className="crm-form">
            <div>
              <label className="crm-label">Nome Completo</label>
              <input
                autoFocus
                type="text"
                className="crm-input"
                value={fullName}
                onChange={(e) => setFullName(e.target.value)}
                required
              />
            </div>

            <div style={{ marginTop: 12 }}>
              <label className="crm-label">Email</label>
              <input
                type="email"
                className="crm-input"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </div>

            <div style={{ marginTop: 12 }}>
              <label className="crm-label">Nível de Acesso</label>
              <select className="crm-input" value={role} onChange={(e) => setRole(e.target.value)}>
                {roles.map((item) => (
                  <option key={item.value} value={item.value}>
                    {item.label}
                  </option>
                ))}
              </select>
            </div>

            {errorMsg && (
              <div className="crm-error" style={{ marginTop: 12 }}>
                {errorMsg}
              </div>
            )}

            <button
              type="submit"
              disabled={isSubmitting}
              className="crm-btn-primary"
              style={{ marginTop: 20, width: '100%' }}
            >
              {isSubmitting ? 'A gravar...' : 'Salvar Usuário'}
            </button>
          </form>
        )}
      </div>
    </div>
  );
}
