import { Link } from "react-router-dom";
import { ArrowLeft, KeyRound } from "lucide-react";

export function ForgotPassword() {
  return (
    <div style={{ display: "flex", justifyContent: "center", alignItems: "center", minHeight: "100vh", background: "#F2F0E9", padding: 16, boxSizing: "border-box", color: "#23291F", fontFamily: "'IBM Plex Sans', sans-serif" }}>
      <section style={{ width: "100%", maxWidth: 390, background: "#FFFFFF", borderLeft: "4px solid #1F3B2C", borderRadius: 4, padding: 28, boxShadow: "0 1px 3px rgba(0,0,0,.06)" }}>
        <KeyRound size={24} color="#1F3B2C" />
        <h1 style={{ fontSize: 21, margin: "14px 0 10px" }}>Esqueci a senha</h1>
        <p style={{ color: "#6E6C61", fontSize: 14, lineHeight: 1.5, margin: "0 0 22px" }}>
          Para proteger sua conta, a redefinição deve ser solicitada ao administrador ou gerente do sistema. Eles poderão gerar uma nova senha temporária para você.
        </p>
        <Link to="/login" style={{ display: "inline-flex", alignItems: "center", gap: 6, color: "#1F3B2C", fontSize: 13, fontWeight: 600, textDecoration: "none" }}>
          <ArrowLeft size={15} /> Voltar para o login
        </Link>
      </section>
    </div>
  );
}
