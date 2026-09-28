import React, { useState } from "react";
import { addSiteToCustomer } from "../services/customerService";
import { X } from "lucide-react";

interface SiteFormModalProps {
  customerId: string;
  onClose: () => void;
  onSuccess: (msg: string) => void;
}

export function SiteFormModal({ customerId, onClose, onSuccess }: SiteFormModalProps) {
  const [form, setForm] = useState({
    name: "",
    country: "Brasil",
    state: "",
    city: "",
  });
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [errorMsg, setErrorMsg] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErrorMsg("");

    if (!form.name.trim()) {
      setErrorMsg("O nome do local é obrigatório.");
      return;
    }

    setIsSubmitting(true);
    try {
      await addSiteToCustomer(customerId, {
        ...form,
        name: form.name.trim(),
      });
      onSuccess("Local Produtivo adicionado com sucesso.");
      onClose();
    } catch (error) {
      console.error(error);
      setErrorMsg("Erro ao adicionar local.");
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="crm-modal-overlay">
      <div className="crm-modal">
        <div className="crm-modal-header">
          <h3 className="crm-modal-title">Novo Local Produtivo</h3>
          <button type="button" onClick={onClose} className="crm-btn-icon">
            <X size={18} />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="crm-form">
          <div>
            <label className="crm-label">Nome da Fazenda / Local</label>
            <input
              autoFocus
              type="text"
              className="crm-input"
              placeholder="Nome da Fazenda/Local"
              value={form.name}
              onChange={(e) => setForm({ ...form, name: e.target.value })}
              required
            />
          </div>

          <div style={{ display: "flex", gap: 10 }}>
            <div style={{ width: "30%" }}>
              <label className="crm-label">Estado (UF)</label>
              <input
                type="text"
                className="crm-input"
                placeholder="UF"
                value={form.state}
                onChange={(e) => setForm({ ...form, state: e.target.value })}
              />
            </div>
            <div style={{ flex: 1 }}>
              <label className="crm-label">Cidade</label>
              <input
                type="text"
                className="crm-input"
                placeholder="Cidade"
                value={form.city}
                onChange={(e) => setForm({ ...form, city: e.target.value })}
              />
            </div>
          </div>

          {errorMsg && <div className="crm-error">{errorMsg}</div>}

          <button
            type="submit"
            disabled={isSubmitting}
            className="crm-btn-primary"
            style={{ marginTop: 6 }}
          >
            {isSubmitting ? "A gravar..." : "Salvar Local"}
          </button>
        </form>
      </div>
    </div>
  );
}