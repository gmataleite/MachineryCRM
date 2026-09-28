import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Building2 } from "lucide-react";
import { useAuth } from "../contexts/AuthContext";
import { api } from "../services/api";

export function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");

    try {
      const response = await api.post("/auth/login", { email, password });

      if (response.data && response.data.token) {
        login(response.data.token);
        navigate("/");
      }
    } catch (err) {
      setError("Credenciais inválidas ou falha de comunicação com o servidor.");
    }
  };

  return (
    <div
      style={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        minHeight: "100vh",
        backgroundColor: "#F2F0E9",
        fontFamily: "'IBM Plex Sans', sans-serif",
        color: "#23291F",
        padding: 16,
        boxSizing: "border-box",
      }}
    >
      <form
        onSubmit={handleSubmit}
        style={{
          padding: "28px 26px",
          background: "#FFFFFF",
          borderLeft: "4px solid #1F3B2C",
          borderRadius: 4,
          boxShadow: "0 1px 3px rgba(0, 0, 0, 0.06)",
          width: "100%",
          maxWidth: 360,
          boxSizing: "border-box",
          textAlign: "left",
        }}
      >
        <div
          style={{
            display: "flex",
            alignItems: "center",
            gap: 8,
            marginBottom: 22,
          }}
        >
          <Building2 size={22} color="#1F3B2C" />
          <h2 style={{ fontSize: 20, fontWeight: 700, margin: 0, color: "#23291F" }}>
            MachineryCRM
          </h2>
        </div>

        {error && (
          <div
            style={{
              background: "#F5E6E4",
              color: "#9E3F33",
              border: "1px solid #E5C5C0",
              padding: "9px 12px",
              borderRadius: 4,
              marginBottom: 16,
              fontSize: 13,
            }}
          >
            {error}
          </div>
        )}

        <div style={{ marginBottom: 16 }}>
          <label
            style={{
              display: "block",
              fontSize: 12,
              fontWeight: 600,
              color: "#6E6C61",
              marginBottom: 6,
            }}
          >
            Email
          </label>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            style={{
              width: "100%",
              padding: "9px 12px",
              background: "#F2F0E9",
              border: "1px solid #DEDCD0",
              borderRadius: 4,
              fontSize: 14,
              color: "#23291F",
              fontFamily: "inherit",
              boxSizing: "border-box",
            }}
            required
          />
        </div>

        <div style={{ marginBottom: 22 }}>
          <label
            style={{
              display: "block",
              fontSize: 12,
              fontWeight: 600,
              color: "#6E6C61",
              marginBottom: 6,
            }}
          >
            Senha
          </label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            style={{
              width: "100%",
              padding: "9px 12px",
              background: "#F2F0E9",
              border: "1px solid #DEDCD0",
              borderRadius: 4,
              fontSize: 14,
              color: "#23291F",
              fontFamily: "inherit",
              boxSizing: "border-box",
            }}
            required
          />
        </div>

        <button
          type="submit"
          style={{
            width: "100%",
            padding: "10px 16px",
            backgroundColor: "#1F3B2C",
            color: "#FFFFFF",
            border: "none",
            borderRadius: 4,
            cursor: "pointer",
            fontSize: 14,
            fontWeight: 600,
            fontFamily: "inherit",
          }}
        >
          Entrar
        </button>
      </form>
    </div>
  );
}