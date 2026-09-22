import React, { useEffect, useState } from 'react';
import { Building2, MapPin, Landmark } from "lucide-react";
import { Link } from 'react-router-dom';
import { getCustomers, createCustomer, type CustomerDto } from "../services/customerService";
import { SiteFormModal } from "../components/SiteFormModal";
import { FiscalFormModal } from "../components/FiscalFormModal";

export function Customers() {
  const [customers, setCustomers] = useState<CustomerDto[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [newCustomerName, setNewCustomerName] = useState<string>("");
  const [successMsg, setSuccessMsg] = useState<string>("");

  const [activeForm, setActiveForm] = useState<{ type: 'site' | 'fiscal' | null, customerId: string | null }>({ type: null, customerId: null });

  const handleModalSuccess = async (msg: string) => {
    showSuccess(msg);
    await fetchCustomers();
  };

  useEffect(() => {
    fetchCustomers();
  }, []);

  const fetchCustomers = async () => {
    try {
      setLoading(true);
      const data = await getCustomers();
      setCustomers(data);
    } catch (error) {
      console.error("Falha ao carregar clientes", error);
    } finally {
      setLoading(false);
    }
  };

  const showSuccess = (msg: string) => {
    setSuccessMsg(msg);
    setTimeout(() => setSuccessMsg(""), 3500);
  };

  const handleSaveCustomer = async () => {
    const trimmedName = newCustomerName.trim();
    if (!trimmedName) return;

    if (customers.some(c => c.name.toLowerCase() === trimmedName.toLowerCase())) {
      if (!window.confirm("Já existe um grupo econômico com este nome. Deseja criar um novo registro mesmo assim?")) return;
    }

    try {
      await createCustomer({ name: trimmedName });
      setNewCustomerName(""); 
      showSuccess("Grupo Econômico registado com sucesso.");
      await fetchCustomers(); 
    } catch (error) {
      console.error("Falha", error);
      alert("Erro ao comunicar com o servidor.");
    }
  };

  if (loading) return <div style={{ padding: 20 }}>A carregar dados do servidor...</div>;

  return (
    <div style={{ fontFamily: "'IBM Plex Sans', sans-serif", background: "#F2F0E9", minHeight: "100vh", color: "#23291F" }}>
        {activeForm.type === 'site' && activeForm.customerId && (
        <SiteFormModal 
          customerId={activeForm.customerId} 
          onClose={() => setActiveForm({ type: null, customerId: null })}
          onSuccess={handleModalSuccess} 
        />
      )}

      {activeForm.type === 'fiscal' && activeForm.customerId && (
        <FiscalFormModal 
          customerId={activeForm.customerId} 
          onClose={() => setActiveForm({ type: null, customerId: null })}
          onSuccess={handleModalSuccess} 
        />
      )}

      <div style={{ maxWidth: 1000, margin: "0 auto", padding: "30px 28px 80px" }}>
        
        <div style={{ display: "flex", alignItems: "center", gap: 8, marginBottom: 20 }}>
          <Building2 size={24} color="#1F3B2C" />
          <h1 style={{ fontSize: 22, fontWeight: 700, margin: 0 }}>Gestão de Clientes</h1>
        </div>
        
        {/* FORMULÁRIO DE NOVO CLIENTE */}
        <div style={{ background: "#FFF", padding: 20, marginBottom: 22, borderLeft: "4px solid #1F3B2C", borderRadius: 4, boxShadow: "0 1px 3px rgba(0,0,0,0.05)" }}>
          <h2 style={{ fontSize: 14, marginBottom: 12, color: "#6E6C61" }}>CADASTRAR NOVO GRUPO ECONÔMICO</h2>
          <div style={{ display: "flex", gap: 10 }}>
            <input type="text" placeholder="Ex: Luis Pereira de Barros e Ricardo Barros" value={newCustomerName} onChange={(e) => setNewCustomerName(e.target.value)} style={{ flex: 1, padding: "9px 10px", border: "1px solid #DEDCD0", borderRadius: 4, fontSize: 14 }} />
            <button onClick={handleSaveCustomer} style={{ background: "#1F3B2C", color: "#fff", border: "none", padding: "9px 20px", borderRadius: 4, cursor: "pointer", fontWeight: 600, fontSize: 14 }}>Registar</button>
          </div>
          {successMsg && <div style={{ marginTop: 10, color: "#2F5240", fontSize: 13.5, fontWeight: 600 }}>✓ {successMsg}</div>}
        </div>

        {/* TABELA DE CLIENTES */}
        <div style={{ background: "#FFF", borderRadius: 4, overflow: "hidden", boxShadow: "0 1px 3px rgba(0,0,0,0.05)" }}>
          <table style={{ width: "100%", borderCollapse: "collapse", textAlign: "left", fontSize: 14 }}>
            <thead>
              <tr style={{ background: "#f8f7f2", borderBottom: "2px solid #DEDCD0" }}>
                <th style={{ padding: "12px 16px", color: "#6E6C61", fontWeight: 600 }}>ID</th>
                <th style={{ padding: "12px 16px", color: "#6E6C61", fontWeight: 600 }}>Nome do Grupo</th>
                <th style={{ padding: "12px 16px", color: "#6E6C61", fontWeight: 600 }}>Locais</th>
                <th style={{ padding: "12px 16px", color: "#6E6C61", fontWeight: 600 }}>Fiscais</th>
                <th style={{ padding: "12px 16px", color: "#6E6C61", fontWeight: 600 }}>Ações Rápidas</th>
              </tr>
            </thead>
            <tbody>
              {customers.length === 0 ? (
                <tr><td colSpan={5} style={{ padding: 20, textAlign: "center", color: "#6E6C61" }}>Nenhum cliente registado.</td></tr>
              ) : (
                customers.map((cliente) => (
                  <React.Fragment key={cliente.id}>
                    <tr style={{ borderBottom: "1px solid #EAE8DD" }}>
                      <td style={{ padding: "12px 16px", fontFamily: "monospace", color: "#888", fontSize: 12 }}>{cliente.id.split('-')[0]}</td>
                      <td style={{ padding: "12px 16px", fontWeight: 600 }}>
                        <Link to={`/customers/${cliente.id}`} style={{ color: "#1F3B2C", textDecoration: "none" }}>
                          {cliente.name}
                        </Link>
                      </td>
                      <td style={{ padding: "12px 16px" }}>{cliente.sites?.length || 0}</td>
                      <td style={{ padding: "12px 16px" }}>{cliente.fiscalEntities?.length || 0}</td>
                      <td style={{ padding: "12px 16px", display: "flex", gap: 8 }}>
                        <button onClick={() => setActiveForm({ type: 'site', customerId: cliente.id })} style={{ background: "#f1f5f9", border: "1px solid #cbd5e1", padding: "4px 8px", borderRadius: 4, cursor: "pointer", display: "flex", alignItems: "center", gap: 4, fontSize: 12 }}><MapPin size={12}/> + Local</button>
                        <button onClick={() => setActiveForm({ type: 'fiscal', customerId: cliente.id })} style={{ background: "#f1f5f9", border: "1px solid #cbd5e1", padding: "4px 8px", borderRadius: 4, cursor: "pointer", display: "flex", alignItems: "center", gap: 4, fontSize: 12 }}><Landmark size={12}/> + Fiscal</button>
                      </td>
                    </tr>
                  </React.Fragment>
                ))
              )}
            </tbody>
          </table>
        </div>

      </div>
    </div>
  );
}