import { useEffect, useState } from 'react';
import { getCustomers, createCustomer, type CustomerDto } from "../services/customerService";
import { Building2 } from "lucide-react";

export function CustomerRegistration() {
  const [customers, setCustomers] = useState<CustomerDto[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [newCustomerName, setNewCustomerName] = useState<string>("");
  const [successMsg, setSuccessMsg] = useState<string>("");

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

  const handleSaveCustomer = async () => {
    const trimmedName = newCustomerName.trim();
    if (!trimmedName) return;

    // Verificação de duplicados no frontend
    const exists = customers.some(c => c.name.toLowerCase() === trimmedName.toLowerCase());
    if (exists) {
      const confirm = window.confirm("Já existe um grupo econômico com este nome. Deseja criar um novo registo mesmo assim?");
      if (!confirm) return;
    }

    try {
      await createCustomer({ name: trimmedName });
      setNewCustomerName(""); 
      
      // Feedback visual
      setSuccessMsg("Grupo Econômico registado com sucesso.");
      setTimeout(() => setSuccessMsg(""), 3500);
      
      await fetchCustomers(); 
    } catch (error) {
      console.error("Falha ao guardar cliente", error);
      alert("Ocorreu um erro ao comunicar com o servidor.");
    }
  };

  if (loading) return <div style={{ padding: 20 }}>A carregar dados do servidor...</div>;

  return (
    <div style={{ fontFamily: "'IBM Plex Sans', sans-serif", background: "#F2F0E9", minHeight: "100vh", color: "#23291F" }}>
      <div style={{ maxWidth: 960, margin: "0 auto", padding: "30px 28px 80px" }}>
        
        <div style={{ display: "flex", alignItems: "center", gap: 8, marginBottom: 20 }}>
          <Building2 size={24} color="#1F3B2C" />
          <h1 style={{ fontSize: 22, fontWeight: 700, margin: 0 }}>Gestão de Clientes</h1>
        </div>
        
        {/* FORMULÁRIO DE NOVO CLIENTE */}
        <div style={{ background: "#FFF", padding: 20, marginBottom: 22, borderLeft: "4px solid #1F3B2C", borderRadius: 4, boxShadow: "0 1px 3px rgba(0,0,0,0.05)" }}>
          <h2 style={{ fontSize: 14, marginBottom: 12, color: "#6E6C61" }}>CADASTRAR NOVO GRUPO ECONÓMICO</h2>
          <div style={{ display: "flex", gap: 10 }}>
            <input 
              type="text" 
              placeholder="Ex: Luis Pereira de Barros e Ricardo Barros"
              value={newCustomerName}
              onChange={(e) => setNewCustomerName(e.target.value)}
              style={{ flex: 1, padding: "9px 10px", border: "1px solid #DEDCD0", borderRadius: 4, fontSize: 14 }}
            />
            <button 
              onClick={handleSaveCustomer}
              style={{ background: "#1F3B2C", color: "#fff", border: "none", padding: "9px 20px", borderRadius: 4, cursor: "pointer", fontWeight: 600, fontSize: 14 }}
            >
              Salvar
            </button>
          </div>
          {successMsg && (
            <div style={{ marginTop: 10, color: "#2F5240", fontSize: 13.5, fontWeight: 600 }}>
              ✓ {successMsg}
            </div>
          )}
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
                <th style={{ padding: "12px 16px", color: "#6E6C61", fontWeight: 600 }}>Contactos</th>
              </tr>
            </thead>
            <tbody>
              {customers.length === 0 ? (
                <tr>
                  <td colSpan={5} style={{ padding: 20, textAlign: "center", color: "#6E6C61" }}>
                    Nenhum cliente registado na base de dados.
                  </td>
                </tr>
              ) : (
                customers.map((cliente) => (
                  <tr key={cliente.id} style={{ borderBottom: "1px solid #EAE8DD" }}>
                    <td style={{ padding: "12px 16px", fontFamily: "monospace", color: "#888", fontSize: 12 }}>
                      {cliente.id.split('-')[0]}
                    </td>
                    <td style={{ padding: "12px 16px", fontWeight: 600, color: "#1F3B2C" }}>
                      {cliente.name}
                    </td>
                    <td style={{ padding: "12px 16px" }}>{cliente.sites?.length || 0}</td>
                    <td style={{ padding: "12px 16px" }}>{cliente.fiscalEntities?.length || 0}</td>
                    <td style={{ padding: "12px 16px" }}>{cliente.contacts?.length || 0}</td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>

      </div>
    </div>
  );
}