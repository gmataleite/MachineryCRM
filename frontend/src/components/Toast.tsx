import { useEffect } from "react";
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
        background: isError ? "#F5E6E4" : "#1F3B2C",
        color: isError ? "#9E3F33" : "#CDE06E",
        border: isError ? "1px solid #E5C5C0" : "1px solid #2F5240",
        padding: "12px 18px",
        borderRadius: 4,
        boxShadow: "0 8px 20px rgba(0,0,0,0.18)",
        fontSize: 13.5,
        fontWeight: 600,
        fontFamily: "'IBM Plex Sans', sans-serif",
        transition: "all 0.2s ease-in-out",
      }}
    >
      {isError ? (
        <AlertCircle size={18} color="#9E3F33" />
      ) : (
        <CheckCircle2 size={18} color="#CDE06E" />
      )}
      <span style={{ color: isError ? "#9E3F33" : "#FFFFFF" }}>{toast.message}</span>
      <button
        type="button"
        onClick={onClose}
        style={{
          background: "none",
          border: "none",
          cursor: "pointer",
          display: "flex",
          alignItems: "center",
          marginLeft: 8,
          padding: 0,
          color: isError ? "#9E3F33" : "#A3B1A8",
        }}
      >
        <X size={15} />
      </button>
    </div>
  );
}