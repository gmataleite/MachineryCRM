import React, { useState } from "react";
import {
  addFiscalEntityToCustomer,
  updateFiscalEntity,
  type FiscalEntityDto,
} from "../services/customerService";
import { X } from "lucide-react";

interface FiscalFormModalProps {
  customerId: string;
  initialData?: FiscalEntityDto | null;
  onClose: () => void;
  onSuccess: (msg: string) => void;
}

export function FiscalFormModal({
  customerId,
  initialData,
  onClose,
  onSuccess,
}: FiscalFormModalProps) {
  const [form, setForm] = useState({
    sapPn: initialData?.sapPn || "",
    name: initialData?.name || "",
    cnpj: initialData?.cnpj || "",
    cpf: initialData?.cpf || "",
    country: initialData?.country || "Brasil",
    state: initialData?.state || "",
    city: initialData?.city || "",
  });
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [errorMsg, setErrorMsg] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErrorMsg("");

    if (!form.name.trim()) {
      setErrorMsg("A Razão Social é obrigatória.");
      return;
    }

    setIsSubmitting(true);
    try {
      const payload = {
        ...form,
        name: form.name.trim(),
      };

      if (initialData) {
        await updateFiscalEntity(initialData.id, payload);
        onClose();
        onSuccess("Ente Fiscal atualizado com sucesso.");
      } else {
        await addFiscalEntityToCustomer(customerId, payload);
        onClose();
        onSuccess("Ente Fiscal adicionado com sucesso.");
      }
    } catch (error) {
      console.error(error);
      setErrorMsg(
        initialData ? "Erro ao atualizar ente fiscal." : "Erro ao adicionar ente fiscal."
      );
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="crm-modal-overlay">
      <div className="crm-modal">
        <div className="crm-modal-header">
          <h3 className="crm-modal-title">
            {initialData ? "Editar Ente Fiscal" : "Novo Ente Fiscal"}
          </h3>
          <button type="button" onClick={onClose} className="crm-btn-icon">
            <X size={18} />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="crm-form">
          <div>
            <label className="crm-label">Razão Social</label>
            <input
              autoFocus
              type="text"
              className="crm-input"
              placeholder="Razão Social"
              value={form.name}
              onChange={(e) => setForm({ ...form, name: e.target.value })}
              required
            />
          </div>

          <div>
            <label className="crm-label">CNPJ / CPF</label>
            <input
              type="text"
              className="crm-input"
              placeholder="CNPJ ou CPF"
              value={form.cnpj}
              onChange={(e) => setForm({ ...form, cnpj: e.target.value })}
            />
          </div>

          <div style={{ display: "flex", gap: 10 }}>
            <div style={{ width: "30%" }}>
              <label className="crm-label">UF</label>
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
            {isSubmitting ? "A gravar..." : "Salvar Ente Fiscal"}
          </button>
        </form>
      </div>
    </div>
  );
}