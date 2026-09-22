import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { getCustomerById, type CustomerDto } from "../services/customerService";
import { Building2, MapPin, Landmark, ArrowLeft, Users } from "lucide-react";
import { SiteFormModal } from "../components/SiteFormModal";
import { FiscalFormModal } from "../components/FiscalFormModal";
import { ContactFormModal } from "../components/ContactFormModal";

export function CustomerDetail() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [customer, setCustomer] = useState<CustomerDto | null>(null);
  const [loading, setLoading] = useState(true);

  // Estado unificado para controle dos modais
  const [modalConfig, setModalConfig] = useState<{
    isOpen: boolean;
    type: 'site' | 'fiscal' | 'contact' | null;
    siteId?: string;
    fiscalEntityId?: string;
  }>({ isOpen: false, type: null });

  useEffect(() => {
    fetchCustomerData();
  }, [id]);

  const fetchCustomerData = async () => {
    if (!id) return;
    try {
      const data = await getCustomerById(id);
      setCustomer(data);
    } catch (err) {
      console.error("Erro", err);
    } finally {
      setLoading(false);
    }
  };

  const handleModalSuccess = async (msg: string) => {
    // Alerta temporário simples. Pode substituir por um toast/snackbar futuramente.
    alert(msg);
    await fetchCustomerData(); // Recarrega os dados do cliente para atualizar a tela
  };

  const closeModal = () => setModalConfig({ isOpen: false, type: null });

  if (loading) return <div style={{ padding: 20 }}>A carregar perfil...</div>;
  if (!customer) return <div style={{ padding: 20 }}>Cliente não encontrado.</div>;

  return (
    <div style={{ fontFamily: "'IBM Plex Sans', sans-serif", background: "#F2F0E9", minHeight: "100vh", color: "#23291F" }}>
      
      {/* RENDERIZAÇÃO DOS MODAIS */}
      {modalConfig.isOpen && modalConfig.type === 'site' && (
        <SiteFormModal customerId={customer.id} onClose={closeModal} onSuccess={handleModalSuccess} />
      )}
      
      {modalConfig.isOpen && modalConfig.type === 'fiscal' && (
        <FiscalFormModal customerId={customer.id} onClose={closeModal} onSuccess={handleModalSuccess} />
      )}

      {modalConfig.isOpen && modalConfig.type === 'contact' && (
        <ContactFormModal 
          customerId={customer.id} 
          siteId={modalConfig.siteId}
          fiscalEntityId={modalConfig.fiscalEntityId}
          onClose={closeModal} 
          onSuccess={handleModalSuccess} 
        />
      )}

      <div style={{ maxWidth: 880, margin: "0 auto", padding: "30px 28px 80px" }}>
        
        <button onClick={() => navigate('/customers')} style={{ background: "none", border: "none", cursor: "pointer", display: "flex", alignItems: "center", gap: 6, color: "#6E6C61", marginBottom: 20, padding: 0 }}>
          <ArrowLeft size={16} /> Voltar para lista
        </button>

        {/* CABEÇALHO DO CLIENTE */}
        <div style={{ background: "#FFF", padding: 20, marginBottom: 22, borderLeft: "4px solid #1F3B2C", borderRadius: 4 }}>
          <div style={{ display: "flex", alignItems: "center", gap: 8, marginBottom: 12 }}>
            <Building2 size={18} color="#1F3B2C" />
            <span style={{ fontSize: 13, color: "#6E6C61", fontWeight: 600, letterSpacing: "0.03em" }}>CLIENTE</span>
            <span style={{ fontSize: 12, color: "#6E6C61", marginLeft: "auto", fontFamily: "monospace" }}>{customer.id}</span>
          </div>
          <div style={{ fontSize: 20, fontWeight: 700, marginBottom: 16 }}>{customer.name}</div>
          
          {/* Contatos Gerais */}
          <div style={{ marginTop: 16, borderTop: "1px solid #EAE8DD", paddingTop: 12 }}>
            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 8 }}>
              <div style={{ display: "flex", alignItems: "center", gap: 6, color: "#6E6C61" }}>
                <Users size={14} /> <span style={{ fontSize: 13, fontWeight: 600 }}>Contatos gerais do cliente</span>
              </div>
              <button 
                onClick={() => setModalConfig({ isOpen: true, type: 'contact' })}
                style={{ background: "#f1f5f9", border: "1px solid #cbd5e1", color: "#1F3B2C", padding: "4px 12px", borderRadius: 4, cursor: "pointer", fontSize: 12, fontWeight: 600 }}
              >
                + Contato
              </button>
            </div>
            {customer.contacts.filter(c => !c.siteId && !c.fiscalEntityId).map(c => (
              <div key={c.id} style={{ background: "#F8F7F2", padding: "8px 12px", borderRadius: 4, fontSize: 13, marginBottom: 4, display: "flex", justifyContent: "space-between" }}>
                <div>
                  <div style={{ fontWeight: 600 }}>{c.description}</div>
                  <div style={{ color: "#6E6C61", marginTop: 2 }}>{c.phone} | {c.email}</div>
                </div>
              </div>
            ))}
          </div>
        </div>

        {/* LOCAIS PRODUTIVOS */}
        <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 12, marginTop: 30 }}>
          <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
            <MapPin size={18} color="#1F3B2C" />
            <h2 style={{ fontSize: 18, margin: 0, fontWeight: 700 }}>Locais produtivos</h2>
          </div>
          <button 
            onClick={() => setModalConfig({ isOpen: true, type: 'site' })}
            style={{ background: "#1F3B2C", color: "#fff", border: "none", padding: "6px 12px", borderRadius: 4, cursor: "pointer", fontSize: 13, fontWeight: 600 }}
          >
            + Novo local
          </button>
        </div>
        
        {customer.sites.map(site => (
          <div key={site.id} style={{ background: "#FFF", padding: 18, marginBottom: 14, borderRadius: 4, border: "1px solid #DEDCD0" }}>
            <div style={{ fontWeight: 700, fontSize: 16 }}>{site.name}</div>
            <div style={{ fontSize: 13, color: "#6E6C61", marginTop: 4 }}>{site.city} — {site.state} — {site.country}</div>
          </div>
        ))}

        {/* ENTES FISCAIS */}
        <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 12, marginTop: 30 }}>
          <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
            <Landmark size={18} color="#1F3B2C" />
            <h2 style={{ fontSize: 18, margin: 0, fontWeight: 700 }}>Entes fiscais</h2>
          </div>
          <button 
            onClick={() => setModalConfig({ isOpen: true, type: 'fiscal' })}
            style={{ background: "#1F3B2C", color: "#fff", border: "none", padding: "6px 12px", borderRadius: 4, cursor: "pointer", fontSize: 13, fontWeight: 600 }}
          >
            + Novo fiscal
          </button>
        </div>

        {customer.fiscalEntities.map(fiscal => (
          <div key={fiscal.id} style={{ background: "#FFF", padding: 18, marginBottom: 14, borderRadius: 4, border: "1px solid #DEDCD0" }}>
            <div style={{ fontWeight: 700, fontSize: 16 }}>{fiscal.name}</div>
            <div style={{ fontSize: 13, color: "#6E6C61", marginTop: 4 }}>CNPJ/CPF: {fiscal.cnpj || fiscal.cpf}</div>
            <div style={{ fontSize: 13, color: "#6E6C61", marginTop: 2 }}>{fiscal.city} — {fiscal.state} — {fiscal.country}</div>
          </div>
        ))}

      </div>
    </div>
  );
}