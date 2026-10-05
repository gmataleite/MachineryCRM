import React, { useState } from "react";
import { addContactToCustomer } from "../services/customerService";
import { X } from "lucide-react";

interface ContactFormModalProps {
  customerId: string;
  siteId?: string;
  fiscalEntityId?: string;
  onClose: () => void;
  onSuccess: (msg: string) => void;
}

export function ContactFormModal({
  customerId,
  siteId,
  fiscalEntityId,
  onClose,
  onSuccess,
}: ContactFormModalProps) {
  const [form, setForm] = useState({ name: "", phone: "", email: "" , observations: "" });
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [errorMsg, setErrorMsg] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErrorMsg("");

    if (!form.name.trim()) {
      setErrorMsg("O nome do contato é obrigatório.");
      return;
    }

    setIsSubmitting(true);
    try {
      await addContactToCustomer(customerId, {
        name: form.name.trim(),
        phone: form.phone.trim() || undefined,
        email: form.email.trim() || undefined,
        observations: form.observations.trim() || undefined,
        siteId,
        fiscalEntityId,
      });
      onSuccess("Contato adicionado com sucesso.");
      onClose();
    } catch (error) {
      console.error(error);
      setErrorMsg("Erro ao adicionar contato.");
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="crm-modal-overlay">
      <div className="crm-modal">
        <div className="crm-modal-header">
          <h3 className="crm-modal-title">Novo Contato</h3>
          <button type="button" onClick={onClose} className="crm-btn-icon">
            <X size={18} />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="crm-form">
          <div>
            <label className="crm-label">Nome / Função</label>
            <input
              autoFocus
              type="text"
              className="crm-input"
              placeholder="Ex: João, Gerente"
              value={form.name}
              onChange={(e) => setForm({ ...form, name: e.target.value })}
              required
            />
          </div>

          <div>
            <label className="crm-label">Telefone</label>
            <input
              type="text"
              className="crm-input"
              placeholder="Ex: (14) 99999-0000"
              value={form.phone}
              onChange={(e) => setForm({ ...form, phone: e.target.value })}
            />
          </div>

          <div>
            <label className="crm-label">E-mail</label>
            <input
              type="email"
              className="crm-input"
              placeholder="Ex: contato@empresa.com.br"
              value={form.email}
              onChange={(e) => setForm({ ...form, email: e.target.value })}
            />
          </div>

          <div>
            <label className="crm-label">Observações</label>
            <input
              type="text"
              className="crm-input"
              placeholder="Ex: Gerente de vendas, responsável por compras"
              value={form.observations}
              onChange={(e) => setForm({ ...form, observations: e.target.value })}
            />
          </div>

          {errorMsg && <div className="crm-error">{errorMsg}</div>}

          <button
            type="submit"
            disabled={isSubmitting}
            className="crm-btn-primary"
            style={{ marginTop: 6 }}
          >
            {isSubmitting ? "A gravar..." : "Salvar Contato"}
          </button>
        </form>
      </div>
    </div>
  );
}