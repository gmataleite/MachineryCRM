import React, { useState } from "react";
import { addSiteToCustomer } from "../services/customerService";
import { X } from "lucide-react";

interface SiteFormModalProps {
  customerId: string;
  onClose: () => void;
  onSuccess: (msg: string) => void;
}

export function SiteFormModal({ customerId, onClose, onSuccess }: SiteFormModalProps) {
  const [form, setForm] = useState({ name: "", country: "Brasil", state: "", city: "" });
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!form.name) return alert("O nome do local é obrigatório.");
    
    setIsSubmitting(true);
    try {
      await addSiteToCustomer(customerId, form);
      onSuccess("Local Produtivo adicionado com sucesso.");
      onClose();
    } catch (error) {
      console.error(error);
      alert("Erro ao adicionar local.");
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div style={{ position: "fixed", top: 0, left: 0, right: 0, bottom: 0, background: "rgba(0,0,0,0.5)", display: "flex", alignItems: "center", justifyContent: "center", zIndex: 1000 }}>
      <div style={{ background: "#FFF", width: 400, borderRadius: 8, padding: 24, boxShadow: "0 4px 12px rgba(0,0,0,0.15)" }}>
        <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 20 }}>
          <h3 style={{ margin: 0, color: "#1F3B2C", fontSize: 16 }}>Novo Local Produtivo</h3>
          <button onClick={onClose} style={{ background: "none", border: "none", cursor: "pointer" }}><X size={20} color="#6E6C61" /></button>
        </div>
        
        <form onSubmit={handleSubmit} style={{ display: "flex", flexDirection: "column", gap: 12 }}>
          <input type="text" placeholder="Nome da Fazenda/Local" value={form.name} onChange={e => setForm({...form, name: e.target.value})} style={{ padding: "10px", border: "1px solid #DEDCD0", borderRadius: 4 }} required />
          <div style={{ display: "flex", gap: 10 }}>
            <input type="text" placeholder="Estado (UF)" value={form.state} onChange={e => setForm({...form, state: e.target.value})} style={{ padding: "10px", border: "1px solid #DEDCD0", borderRadius: 4, width: "30%" }} />
            <input type="text" placeholder="Cidade" value={form.city} onChange={e => setForm({...form, city: e.target.value})} style={{ padding: "10px", border: "1px solid #DEDCD0", borderRadius: 4, flex: 1 }} />
          </div>
          <button type="submit" disabled={isSubmitting} style={{ background: "#1F3B2C", color: "#fff", border: "none", padding: "10px", borderRadius: 4, cursor: isSubmitting ? "not-allowed" : "pointer", fontWeight: 600, marginTop: 10 }}>
            {isSubmitting ? "A gravar..." : "Salvar Local"}
          </button>
        </form>
      </div>
    </div>
  );
}