import React, { createContext, useContext, useState, useEffect } from 'react';

interface AuthContextType {
  isAuthenticated: boolean;
  role: string | null;
  login: (token: string, remember: boolean) => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const [role, setRole] = useState<string | null>(null);

  useEffect(() => {
    const token = localStorage.getItem("jwt_token") ?? sessionStorage.getItem("jwt_token");
    setIsAuthenticated(!!token);
    setRole(token ? getRoleFromToken(token) : null);
  }, []);

  const login = (token: string, remember: boolean) => {
    const storage = remember ? localStorage : sessionStorage;
    localStorage.removeItem("jwt_token");
    sessionStorage.removeItem("jwt_token");
    storage.setItem("jwt_token", token);
    setIsAuthenticated(true);
    setRole(getRoleFromToken(token));
  };

  const logout = () => {
    localStorage.removeItem("jwt_token");
    sessionStorage.removeItem("jwt_token");
    setIsAuthenticated(false);
    setRole(null);
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, role, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

function getRoleFromToken(token: string): string | null {
  try {
    const payload = token.split(".")[1];
    const base64 = payload.replace(/-/g, "+").replace(/_/g, "/");
    const padded = base64.padEnd(base64.length + ((4 - (base64.length % 4)) % 4), "=");
    const decoded = JSON.parse(atob(padded));
    return decoded.role ?? decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ?? null;
  } catch {
    return null;
  }
}

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};