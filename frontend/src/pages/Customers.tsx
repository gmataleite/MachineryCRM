import { useEffect, useMemo, useState } from "react";
import { Building2, ArrowUp, ArrowDown, ArrowUpDown } from "lucide-react";
import { Link } from "react-router-dom";
import { getCustomers, createCustomer, type CustomerDto } from "../services/customerService";
import { Toast, type ToastData } from "../components/Toast";

type SortKey = "id" | "name" | "sites" | "fiscalEntities";
type SortDirection = "asc" | "desc";

export function Customers() {
  const [customers, setCustomers] = useState<CustomerDto[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [newCustomerName, setNewCustomerName] = useState<string>("");
  const [toast, setToast] = useState<ToastData | null>(null);
  const [sortConfig, setSortConfig] = useState<{
    key: SortKey;
    direction: SortDirection;
  }>({
    key: "name",
    direction: "asc",
  });

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
    setToast({ message: msg, type: "success" });
    setTimeout(() => setToast(null), 3500);
  };

  const handleSaveCustomer = async () => {
    const trimmedName = newCustomerName.trim();
    if (!trimmedName) return;

    if (customers.some((c) => c.name.toLowerCase() === trimmedName.toLowerCase())) {
      if (
        !window.confirm(
          "Já existe um grupo econômico com este nome. Deseja criar um novo registro mesmo assim?"
        )
      )
        return;
    }

    try {
      await createCustomer({ name: trimmedName });
      setNewCustomerName("");
      showSuccess("Grupo Econômico registrado com sucesso.");
      await fetchCustomers();
    } catch (error) {
      console.error("Falha", error);
      alert("Erro ao comunicar com o servidor.");
    }
  };

  const handleSort = (key: SortKey) => {
    setSortConfig((prev) => ({
      key,
      direction: prev.key === key && prev.direction === "asc" ? "desc" : "asc",
    }));
  };

  const sortedCustomers = useMemo(() => {
    const items = [...customers];

    items.sort((a, b) => {
      let comparison = 0;

      switch (sortConfig.key) {
        case "id":
          comparison = a.id
            .split("-")[0]
            .localeCompare(b.id.split("-")[0], "pt", { sensitivity: "base" });
          break;
        case "name":
          comparison = a.name.localeCompare(b.name, "pt", { sensitivity: "base" });
          break;
        case "sites":
          comparison = (a.sites?.length || 0) - (b.sites?.length || 0);
          break;
        case "fiscalEntities":
          comparison = (a.fiscalEntities?.length || 0) - (b.fiscalEntities?.length || 0);
          break;
      }

      return sortConfig.direction === "asc" ? comparison : -comparison;
    });

    return items;
  }, [customers, sortConfig]);

  const renderSortIcon = (key: SortKey) => {
    if (sortConfig.key !== key) {
      return <ArrowUpDown size={13} color="#9E9B8F" />;
    }
    return sortConfig.direction === "asc" ? (
      <ArrowUp size={13} color="#1F3B2C" />
    ) : (
      <ArrowDown size={13} color="#1F3B2C" />
    );
  };

  const sortableHeaderStyle: React.CSSProperties = {
    cursor: "pointer",
    userSelect: "none",
  };

  if (loading) return <div className="crm-status">A carregar dados do servidor...</div>;

  return (
    <div>
      <Toast toast={toast} onClose={() => setToast(null)} />

      <div className="crm-page-header">
        <Building2 size={24} color="#1F3B2C" />
        <h1 className="crm-page-title">Gestão de Clientes</h1>
      </div>

      <div className="crm-card-accent">
        <h2
          style={{
            fontSize: 13,
            fontWeight: 700,
            letterSpacing: "0.04em",
            marginTop: 0,
            marginBottom: 12,
            color: "#6E6C61",
          }}
        >
          CADASTRAR NOVO GRUPO ECONÔMICO
        </h2>
        <div style={{ display: "flex", gap: 10 }}>
          <input
            type="text"
            className="crm-input"
            placeholder="Ex: Luis Pereira de Barros e Ricardo Barros"
            value={newCustomerName}
            onChange={(e) => setNewCustomerName(e.target.value)}
            style={{ flex: 1 }}
          />
          <button
            type="button"
            onClick={handleSaveCustomer}
            className="crm-btn-primary"
            style={{ padding: "9px 20px", fontSize: 14 }}
          >
            Registrar
          </button>
        </div>
      </div>

      <div className="crm-table-wrapper">
        <table className="crm-table">
          <thead>
            <tr>
              <th onClick={() => handleSort("id")} style={sortableHeaderStyle}>
                <span style={{ display: "inline-flex", alignItems: "center", gap: 6 }}>
                  ID {renderSortIcon("id")}
                </span>
              </th>
              <th onClick={() => handleSort("name")} style={sortableHeaderStyle}>
                <span style={{ display: "inline-flex", alignItems: "center", gap: 6 }}>
                  Nome do Grupo {renderSortIcon("name")}
                </span>
              </th>
              <th
                onClick={() => handleSort("sites")}
                style={{ ...sortableHeaderStyle, textAlign: "center" }}
              >
                <span
                  style={{
                    display: "inline-flex",
                    alignItems: "center",
                    justifyContent: "center",
                    gap: 6,
                  }}
                >
                  Locais {renderSortIcon("sites")}
                </span>
              </th>
              <th
                onClick={() => handleSort("fiscalEntities")}
                style={{ ...sortableHeaderStyle, textAlign: "center" }}
              >
                <span
                  style={{
                    display: "inline-flex",
                    alignItems: "center",
                    justifyContent: "center",
                    gap: 6,
                  }}
                >
                  Fiscais {renderSortIcon("fiscalEntities")}
                </span>
              </th>
            </tr>
          </thead>
          <tbody>
            {sortedCustomers.length === 0 ? (
              <tr>
                <td colSpan={4} className="crm-table-empty">
                  Nenhum cliente registado.
                </td>
              </tr>
            ) : (
              sortedCustomers.map((cliente) => (
                <tr key={cliente.id}>
                  <td className="crm-mono-id">
                    {cliente.id.split("-")[0].toUpperCase()}
                  </td>
                  <td style={{ fontWeight: 600 }}>
                    <Link
                      to={`/customers/${cliente.id}`}
                      style={{ color: "#1F3B2C", textDecoration: "none" }}
                    >
                      {cliente.name}
                    </Link>
                  </td>
                  <td style={{ textAlign: "center" }}>{cliente.sites?.length || 0}</td>
                  <td style={{ textAlign: "center" }}>{cliente.fiscalEntities?.length || 0}</td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}