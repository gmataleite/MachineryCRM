import { useState } from "react";
import { KeyRound } from "lucide-react";
import { api } from "../services/api";

export function ChangePassword() {
  const [currentPassword, setCurrentPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmation, setConfirmation] = useState("");
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  async function handleSubmit(event: React.FormEvent) {
    event.preventDefault();
    setError("");
    setMessage("");
    if (newPassword.length < 8) return setError("A nova senha deve ter pelo menos 8 caracteres.");
    if (newPassword !== confirmation) return setError("A confirmação não corresponde à nova senha.");
    try {
      await api.post("/auth/change-password", { currentPassword, newPassword });
      setCurrentPassword("");
      setNewPassword("");
      setConfirmation("");
      setMessage("Senha alterada com sucesso.");
    } catch {
      setError("Não foi possível alterar a senha. Confira sua senha atual.");
    }
  }

  return (
    <section style={{ maxWidth: 620 }}>
      <h1 style={{ display: "flex", alignItems: "center", gap: 10, margin: "0 0 6px", fontSize: 26 }}><KeyRound size={24} color="#1F3B2C" /> Alterar senha</h1>
      <p style={{ color: "#6E6C61", margin: "0 0 24px", fontSize: 14 }}>Use uma senha com pelo menos 8 caracteres.</p>
      {error && <div style={alertStyle}>{error}</div>}
      {message && <div style={successStyle}>{message}</div>}
      <form onSubmit={handleSubmit} style={cardStyle}>
        <label style={labelStyle}>Senha atual</label>
        <input type="password" value={currentPassword} onChange={(event) => setCurrentPassword(event.target.value)} required style={inputStyle} />
        <label style={{ ...labelStyle, marginTop: 16 }}>Nova senha</label>
        <input type="password" value={newPassword} onChange={(event) => setNewPassword(event.target.value)} required minLength={8} style={inputStyle} />
        <label style={{ ...labelStyle, marginTop: 16 }}>Confirmar nova senha</label>
        <input type="password" value={confirmation} onChange={(event) => setConfirmation(event.target.value)} required style={inputStyle} />
        <button type="submit" style={buttonStyle}>Salvar nova senha</button>
      </form>
    </section>
  );
}

const cardStyle = { background: "#FFFFFF", padding: 24, borderRadius: 4, boxShadow: "0 1px 3px rgba(0,0,0,.06)" };
const labelStyle = { display: "block", fontSize: 12, fontWeight: 600, color: "#6E6C61", marginBottom: 6 };
const inputStyle = { width: "100%", padding: "10px 12px", background: "#F2F0E9", border: "1px solid #DEDCD0", borderRadius: 4, fontSize: 14, color: "#23291F", fontFamily: "inherit", boxSizing: "border-box" as const };
const buttonStyle = { width: "100%", marginTop: 22, padding: "10px 16px", background: "#1F3B2C", color: "#FFFFFF", border: 0, borderRadius: 4, cursor: "pointer", fontSize: 14, fontWeight: 600, fontFamily: "inherit" };
const alertStyle = { background: "#F5E6E4", color: "#9E3F33", border: "1px solid #E5C5C0", padding: "9px 12px", borderRadius: 4, marginBottom: 16, fontSize: 13 };
const successStyle = { background: "#E6F0E8", color: "#1F3B2C", border: "1px solid #B8D0BC", padding: "9px 12px", borderRadius: 4, marginBottom: 16, fontSize: 13 };
