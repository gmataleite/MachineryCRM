import React, { useState, useEffect, useRef } from "react";
import { APIProvider, useMapsLibrary } from "@vis.gl/react-google-maps";
import { addSiteToCustomer, updateSite, type SiteDto } from "../services/customerService";
import { X, MapPin } from "lucide-react";

const API_KEY = import.meta.env.VITE_GOOGLE_MAPS_API_KEY || "";

interface SiteFormModalProps {
  customerId: string;
  initialData?: SiteDto | null;
  onClose: () => void;
  onSuccess: (msg: string) => void;
}

// Subcomponente restrito a buscar apenas cidades na API do Google
function CityAutocomplete({
  value,
  onChange,
  onLocationExtracted,
}: {
  value: string;
  onChange: (val: string) => void;
  onLocationExtracted: (city: string, state: string, country: string) => void;
}) {
  const inputRef = useRef<HTMLInputElement>(null);
  const placesLib = useMapsLibrary("places");
  const extractorRef = useRef(onLocationExtracted);

  useEffect(() => {
    extractorRef.current = onLocationExtracted;
  }, [onLocationExtracted]);

  useEffect(() => {
    if (!placesLib || !inputRef.current) return;

    const autocomplete = new placesLib.Autocomplete(inputRef.current, {
      fields: ["address_components"],
      types: ["(cities)"],
    });

    const listener = autocomplete.addListener("place_changed", () => {
      const place = autocomplete.getPlace();
      if (place.address_components) {
        let city = "";
        let state = "";
        let country = "";

        for (const component of place.address_components) {
          const types = component.types;
          if (types.includes("administrative_area_level_2") || types.includes("locality")) {
            city = component.long_name;
          }
          if (types.includes("administrative_area_level_1")) {
            state = component.short_name; // Ex: SP
          }
          if (types.includes("country")) {
            country = component.short_name; // Ex: BR
          }
        }
        extractorRef.current(city, state, country);
      }
    });

    return () => listener.remove();
  }, [placesLib]);

  return (
    <div style={{ position: "relative", display: "flex", alignItems: "center" }}>
      <MapPin size={15} color="#6E6C61" style={{ position: "absolute", left: 10, pointerEvents: "none" }} />
      <input
        ref={inputRef}
        type="text"
        className="crm-input"
        placeholder="Buscar cidade..."
        value={value}
        onChange={(e) => onChange(e.target.value)}
        onKeyDown={(e) => {
          if (e.key === "Enter") e.preventDefault();
        }}
        style={{ paddingLeft: 32 }}
      />
    </div>
  );
}

export function SiteFormModal({ customerId, initialData, onClose, onSuccess }: SiteFormModalProps) {
  const [form, setForm] = useState({
    name: initialData?.name || "",
    address: {
      city: initialData?.address?.city || "",
      state: initialData?.address?.state || "",
      countryCode: initialData?.address?.countryCode || "BR",
    },
    observations: initialData?.observations || "",
  });
  
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [errorMsg, setErrorMsg] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErrorMsg("");

    if (!form.name.trim()) {
      setErrorMsg("O nome do local é obrigatório.");
      return;
    }

    if (!form.address.city.trim() || !form.address.state.trim()) {
      setErrorMsg("Cidade e Estado são obrigatórios.");
      return;
    }

    setIsSubmitting(true);
    try {
      const payload = {
        name: form.name.trim(),
        address: {
          city: form.address.city.trim() || null,
          state: form.address.state.trim() || null,
          countryCode: form.address.countryCode.trim() || null,
        },
        observations: form.observations.trim(),
      };

      if (initialData) {
        await updateSite(initialData.id, payload);
        onClose();
        onSuccess("Local produtivo atualizado com sucesso.");
      } else {
        await addSiteToCustomer(customerId, payload);
        onClose();
        onSuccess("Local produtivo adicionado com sucesso.");
      }
    } catch (error) {
      console.error(error);
      setErrorMsg(initialData ? "Erro ao atualizar local." : "Erro ao adicionar local.");
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="crm-modal-overlay">
      <div className="crm-modal">
        <div className="crm-modal-header">
          <h3 className="crm-modal-title">
            {initialData ? "Editar Local Produtivo" : "Novo Local Produtivo"}
          </h3>
          <button type="button" onClick={onClose} className="crm-btn-icon">
            <X size={18} />
          </button>
        </div>

        <APIProvider apiKey={API_KEY}>
          <form onSubmit={handleSubmit} className="crm-form">
            <div>
              <label className="crm-label">Nome do Local (Ex: Sede, Fazenda Bela Vista)</label>
              <input
                autoFocus
                type="text"
                className="crm-input"
                placeholder="Nome do local"
                value={form.name}
                onChange={(e) => setForm({ ...form, name: e.target.value })}
                required
              />
            </div>

            <div style={{ marginTop: 12 }}>
              <label className="crm-label">Cidade</label>
              <CityAutocomplete
                value={form.address.city}
                onChange={(val) => setForm({ ...form, address: { ...form.address, city: val } })}
                onLocationExtracted={(city, state, country) => {
                  setForm((prev) => ({
                    ...prev,
                    address: {
                      ...prev.address,
                      city: city || prev.address.city,
                      state: state || prev.address.state,
                      countryCode: country || prev.address.countryCode,
                    },
                  }));
                }}
              />
            </div>

            <div style={{ display: "flex", gap: 10, marginTop: 12 }}>
              <div style={{ width: "40%" }}>
                <label className="crm-label">Estado (UF)</label>
                <input
                  type="text"
                  className="crm-input"
                  placeholder="UF"
                  value={form.address.state}
                  onChange={(e) => setForm({ ...form, address: { ...form.address, state: e.target.value } })}
                  required
                />
              </div>
              <div style={{ flex: 1 }}>
                <label className="crm-label">País</label>
                <input
                  type="text"
                  className="crm-input"
                  placeholder="País"
                  value={form.address.countryCode}
                  onChange={(e) => setForm({ ...form, address: { ...form.address, countryCode: e.target.value } })}
                  required
                />
              </div>
            </div>

            <div style={{ marginTop: 12 }}>
              <label className="crm-label">Observações</label>
              <textarea
                className="crm-input"
                rows={2}
                placeholder="Detalhes adicionais..."
                value={form.observations}
                onChange={(e) => setForm({ ...form, observations: e.target.value })}
              />
            </div>

            {errorMsg && <div className="crm-error" style={{ marginTop: 10 }}>{errorMsg}</div>}

            <button
              type="submit"
              disabled={isSubmitting}
              className="crm-btn-primary"
              style={{ marginTop: 16, width: "100%" }}
            >
              {isSubmitting ? "A gravar..." : "Salvar Local Produtivo"}
            </button>
          </form>
        </APIProvider>
      </div>
    </div>
  );
}