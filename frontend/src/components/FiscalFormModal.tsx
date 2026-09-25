import React, { useState } from "react";
import { addFiscalEntityToCustomer } from "../services/customerService";
import { X } from "lucide-react";

interface FiscalFormModalProps {
  customerId: string;
  onClose: () => void;
  onSuccess: (msg: string) => void;
}

export function FiscalFormModal({ customerId, onClose, onSuccess }: FiscalFormModalProps) {
  const [form, setForm] = useState({ name: "", cnpj: "", cpf: "", country: "Brasil", state: "", city: "" });
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!form.name) return alert("A Razão Social é obrigatória.");
    
    setIsSubmitting(true);
    try {
      await addFiscalEntityToCustomer(customerId, form);
      onSuccess("Ente Fiscal adicionado com sucesso.");
      onClose();
    } catch (error) {
      console.error(error);
      alert("Erro ao adicionar ente fiscal.");
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div style={{ position: "fixed", top: 0, left: 0, right: 0, bottom: 0, background: "rgba(0,0,0,0.5)", display: "flex", alignItems: "center", justifyContent: "center", zIndex: 1000 }}>
      <div style={{ background: "#FFF", width: 400, borderRadius: 8, padding: 24, boxShadow: "0 4px 12px rgba(0,0,0,0.15)" }}>
        <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 20 }}>
          <h3 style={{ margin: 0, color: "#1F3B2C", fontSize: 16 }}>Novo Ente Fiscal</h3>
          <button onClick={onClose} style={{ background: "none", border: "none", cursor: "pointer" }}><X size={20} color="#6E6C61" /></button>
        </div>
        
        <form onSubmit={handleSubmit} style={{ display: "flex", flexDirection: "column", gap: 12 }}>
          <input autoFocus type="text" placeholder="Razão Social" value={form.name} onChange={e => setForm({...form, name: e.target.value})} style={{ padding: "10px", border: "1px solid #DEDCD0", borderRadius: 4 }} required />
          <input type="text" placeholder="CNPJ/CPF" value={form.cnpj} onChange={e => setForm({...form, cnpj: e.target.value})} style={{ padding: "10px", border: "1px solid #DEDCD0", borderRadius: 4 }} />
          <div style={{ display: "flex", gap: 10 }}>
            <input type="text" placeholder="UF" value={form.state} onChange={e => setForm({...form, state: e.target.value})} style={{ padding: "10px", border: "1px solid #DEDCD0", borderRadius: 4, width: "30%" }} />
            <input type="text" placeholder="Cidade" value={form.city} onChange={e => setForm({...form, city: e.target.value})} style={{ padding: "10px", border: "1px solid #DEDCD0", borderRadius: 4, flex: 1 }} />
          </div>
          <button type="submit" disabled={isSubmitting} style={{ background: "#1F3B2C", color: "#fff", border: "none", padding: "10px", borderRadius: 4, cursor: isSubmitting ? "not-allowed" : "pointer", fontWeight: 600, marginTop: 10 }}>
            {isSubmitting ? "A gravar..." : "Salvar Ente Fiscal"}
          </button>
        </form>
      </div>
    </div>
  );
}