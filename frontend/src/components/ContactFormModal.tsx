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

export function ContactFormModal({ customerId, siteId, fiscalEntityId, onClose, onSuccess }: ContactFormModalProps) {
  const [form, setForm] = useState({ description: "", phone: "", email: "" });
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!form.description) return alert("A descrição/nome do contato é obrigatória.");
    
    setIsSubmitting(true);
    try {
      await addContactToCustomer(customerId, {
        description: form.description,
        phone: form.phone,
        email: form.email,
        siteId: siteId,
        fiscalEntityId: fiscalEntityId
      });
      onSuccess("Contato adicionado com sucesso.");
      onClose();
    } catch (error) {
      console.error(error);
      alert("Erro ao adicionar contato.");
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div style={{ position: "fixed", top: 0, left: 0, right: 0, bottom: 0, background: "rgba(0,0,0,0.5)", display: "flex", alignItems: "center", justifyContent: "center", zIndex: 1000 }}>
      <div style={{ background: "#FFF", width: 400, borderRadius: 8, padding: 24, boxShadow: "0 4px 12px rgba(0,0,0,0.15)" }}>
        <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 20 }}>
          <h3 style={{ margin: 0, color: "#1F3B2C", fontSize: 16 }}>Novo Contato</h3>
          <button onClick={onClose} style={{ background: "none", border: "none", cursor: "pointer" }}><X size={20} color="#6E6C61" /></button>
        </div>
        
        <form onSubmit={handleSubmit} style={{ display: "flex", flexDirection: "column", gap: 12 }}>
          <input type="text" placeholder="Nome / Função (Ex: João, Gerente)" value={form.description} onChange={e => setForm({...form, description: e.target.value})} style={{ padding: "10px", border: "1px solid #DEDCD0", borderRadius: 4 }} required />
          <input type="text" placeholder="Telefone" value={form.phone} onChange={e => setForm({...form, phone: e.target.value})} style={{ padding: "10px", border: "1px solid #DEDCD0", borderRadius: 4 }} />
          <input type="email" placeholder="E-mail" value={form.email} onChange={e => setForm({...form, email: e.target.value})} style={{ padding: "10px", border: "1px solid #DEDCD0", borderRadius: 4 }} />
          
          <button type="submit" disabled={isSubmitting} style={{ background: "#1F3B2C", color: "#fff", border: "none", padding: "10px", borderRadius: 4, cursor: isSubmitting ? "not-allowed" : "pointer", fontWeight: 600, marginTop: 10 }}>
            {isSubmitting ? "A gravar..." : "Salvar Contato"}
          </button>
        </form>
      </div>
    </div>
  );
}