import React, { useEffect } from "react";
import { CheckCircle2, AlertCircle, X } from "lucide-react";

export interface ToastData {
  message: string;
  type?: "success" | "error";
}

interface ToastProps {
  toast: ToastData | null;
  onClose: () => void;
  duration?: number;
}

export function Toast({ toast, onClose, duration = 3500 }: ToastProps) {
  useEffect(() => {
    if (!toast) return;
    const timer = setTimeout(() => {
      onClose();
    }, duration);

    return () => clearTimeout(timer);
  }, [toast, onClose, duration]);

  if (!toast) return null;

  const isError = toast.type === "error";

  return (
    <div
      style={{
        position: "fixed",
        bottom: 24,
        right: 24,
        zIndex: 9999,
        display: "flex",
        alignItems: "center",
        gap: 10,
        background: isError ? "#FEF2F2" : "#1F3B2C",
        color: isError ? "#991B1B" : "#CDE06E", // Usando o verde RGB(205, 224, 110) no destaque
        border: isError ? "1px solid #F87171" : "1px solid #2F5240",
        padding: "12px 18px",
        borderRadius: 6,
        boxShadow: "0 8px 20px rgba(0,0,0,0.18)",
        fontSize: 13.5,
        fontWeight: 600,
        fontFamily: "'IBM Plex Sans', sans-serif",
        transition: "all 0.2s ease-in-out",
      }}
    >
      {isError ? (
        <AlertCircle size={18} color="#DC2626" />
      ) : (
        <CheckCircle2 size={18} color="#CDE06E" />
      )}
      <span style={{ color: isError ? "#991B1B" : "#FFFFFF" }}>{toast.message}</span>
      <button
        onClick={onClose}
        style={{
          background: "none",
          border: "none",
          cursor: "pointer",
          display: "flex",
          alignItems: "center",
          marginLeft: 8,
          padding: 0,
          color: isError ? "#991B1B" : "#A3B1A8",
        }}
      >
        <X size={15} />
      </button>
    </div>
  );
}