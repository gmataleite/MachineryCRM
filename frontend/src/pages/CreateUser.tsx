import { useState } from "react";
import { ArrowLeft, Building2, Copy, UserPlus } from "lucide-react";
import { Link } from "react-router-dom";
import { api } from "../services/api";

const roles = [
  { value: "Admin", label: "Administrador" },
  { value: "Manager", label: "Gerente" },
  { value: "Sales", label: "Vendas" },
  { value: "Technician", label: "Técnico" },
];

export function CreateUser() {
  const [fullName, setFullName] = useState("");
  const [email, setEmail] = useState("");
  const [role, setRole] = useState("Technician");
  const [temporaryPassword, setTemporaryPassword] = useState("");
  const [error, setError] = useState("");
  const [saved, setSaved] = useState(false);

  async function handleSubmit(event: React.FormEvent) {
    event.preventDefault();
    setError("");
    setSaved(false);
    try {
      const response = await api.post("/appusers", { fullName, email, role });
      setTemporaryPassword(response.data.temporaryPassword);
      setSaved(true);
    } catch {
      setError("Não foi possível cadastrar o usuário. Verifique os dados e tente novamente.");
    }
  }

  return (
    <section style={{ maxWidth: 620 }}>
      <Link to="/" style={{ display: "inline-flex", alignItems: "center", gap: 6, color: "#6E6C61", textDecoration: "none", fontSize: 13, marginBottom: 18 }}>
        <ArrowLeft size={15} /> Voltar
      </Link>
      <h1 style={{ display: "flex", alignItems: "center", gap: 10, margin: "0 0 6px", fontSize: 26 }}>
        <UserPlus size={24} color="#1F3B2C" /> Novo usuário
      </h1>
      <p style={{ color: "#6E6C61", margin: "0 0 24px", fontSize: 14 }}>Cadastre um usuário e defina o nível de acesso inicial.</p>

      {error && <div style={alertStyle}>{error}</div>}
      {saved ? (
        <div style={{ ...cardStyle, borderLeft: "4px solid #1F3B2C" }}>
          <h2 style={{ margin: "0 0 8px", fontSize: 18 }}>Usuário cadastrado</h2>
          <p style={{ margin: "0 0 18px", color: "#6E6C61", fontSize: 14 }}>Entregue esta senha temporária ao usuário. Ela deverá ser alterada no primeiro acesso.</p>
          <div style={{ display: "flex", alignItems: "center", gap: 10, background: "#F2F0E9", padding: "12px 14px", borderRadius: 4 }}>
            <strong style={{ flex: 1, letterSpacing: 1 }}>{temporaryPassword}</strong>
            <button type="button" onClick={() => navigator.clipboard.writeText(temporaryPassword)} style={secondaryButtonStyle}><Copy size={15} /> Copiar</button>
          </div>
          <button type="button" onClick={() => { setSaved(false); setFullName(""); setEmail(""); setTemporaryPassword(""); }} style={{ ...primaryButtonStyle, marginTop: 18 }}>Cadastrar outro usuário</button>
        </div>
      ) : (
        <form onSubmit={handleSubmit} style={cardStyle}>
          <div style={{ marginBottom: 16 }}>
            <label style={labelStyle}>Nome completo</label>
            <input value={fullName} onChange={(event) => setFullName(event.target.value)} required maxLength={200} style={inputStyle} />
          </div>
          <div style={{ marginBottom: 16 }}>
            <label style={labelStyle}>Email</label>
            <input type="email" value={email} onChange={(event) => setEmail(event.target.value)} required maxLength={150} style={inputStyle} />
          </div>
          <div style={{ marginBottom: 22 }}>
            <label style={labelStyle}>Tipo de acesso</label>
            <select value={role} onChange={(event) => setRole(event.target.value)} style={inputStyle}>
              {roles.map((item) => <option key={item.value} value={item.value}>{item.label}</option>)}
            </select>
          </div>
          <button type="submit" style={primaryButtonStyle}><Building2 size={16} /> Criar usuário</button>
        </form>
      )}
    </section>
  );
}

const cardStyle = { background: "#FFFFFF", padding: 24, borderRadius: 4, boxShadow: "0 1px 3px rgba(0,0,0,.06)" };
const labelStyle = { display: "block", fontSize: 12, fontWeight: 600, color: "#6E6C61", marginBottom: 6 };
const inputStyle = { width: "100%", padding: "10px 12px", background: "#F2F0E9", border: "1px solid #DEDCD0", borderRadius: 4, fontSize: 14, color: "#23291F", fontFamily: "inherit", boxSizing: "border-box" as const };
const primaryButtonStyle = { display: "inline-flex", alignItems: "center", justifyContent: "center", gap: 8, width: "100%", padding: "10px 16px", background: "#1F3B2C", color: "#FFFFFF", border: 0, borderRadius: 4, cursor: "pointer", fontSize: 14, fontWeight: 600, fontFamily: "inherit" };
const secondaryButtonStyle = { display: "inline-flex", alignItems: "center", gap: 6, padding: "7px 10px", background: "#FFFFFF", color: "#1F3B2C", border: "1px solid #1F3B2C", borderRadius: 4, cursor: "pointer", fontFamily: "inherit" };
const alertStyle = { background: "#F5E6E4", color: "#9E3F33", border: "1px solid #E5C5C0", padding: "9px 12px", borderRadius: 4, marginBottom: 16, fontSize: 13 };
