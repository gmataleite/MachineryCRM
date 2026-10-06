import React, { useState, useEffect, useRef } from "react";
import { APIProvider, useMapsLibrary } from "@vis.gl/react-google-maps";
import {
  addFiscalEntityToCustomer,
  updateFiscalEntity,
  type FiscalEntityDto,
  type AddressDto,
} from "../services/customerService";
import { X } from "lucide-react";

const API_KEY = import.meta.env.VITE_GOOGLE_MAPS_API_KEY || "";

interface FiscalFormModalProps {
  customerId: string;
  initialData?: FiscalEntityDto | null;
  onClose: () => void;
  onSuccess: (msg: string) => void;
}

// Subcomponente que transforma um input comum num campo com Autocomplete do Google
function AutocompleteInput({
  value,
  onChange,
  onAddressExtracted,
  placeholder,
}: {
  value: string | null | undefined;
  onChange: (val: string) => void;
  onAddressExtracted: (address: AddressDto) => void;
  placeholder?: string;
}) {
  const inputRef = useRef<HTMLInputElement>(null);
  const placesLib = useMapsLibrary("places");
  const onAddressExtractedRef = useRef(onAddressExtracted);

  useEffect(() => {
    onAddressExtractedRef.current = onAddressExtracted;
  }, [onAddressExtracted]);

  useEffect(() => {
    if (!placesLib || !inputRef.current) return;

    const autocomplete = new placesLib.Autocomplete(inputRef.current, {
      fields: ["address_components"],
      componentRestrictions: { country: "br" }, // Restrito ao Brasil (opcional)
    });

    const listener = autocomplete.addListener("place_changed", () => {
      const place = autocomplete.getPlace();

      if (place.address_components) {
        let route = "";
        let streetNumber = "";
        const parsedAddress: AddressDto = {
          addressLine: "",
          neighborhood: "",
          city: "",
          state: "",
          postalCode: "",
          countryCode: "",
        };

        for (const component of place.address_components) {
          const types = component.types;

          if (types.includes("route")) route = component.long_name;
          if (types.includes("street_number")) streetNumber = component.long_name;

          if (types.includes("sublocality") || types.includes("sublocality_level_1")) {
            parsedAddress.neighborhood = component.long_name;
          }
          if (types.includes("administrative_area_level_2") || types.includes("locality")) {
            parsedAddress.city = component.long_name;
          }
          if (types.includes("administrative_area_level_1")) {
            parsedAddress.state = component.short_name;
          }
          if (types.includes("postal_code")) {
            parsedAddress.postalCode = component.long_name;
          }
          if (types.includes("country")) {
            parsedAddress.countryCode = component.short_name;
          }
        }

        parsedAddress.addressLine = `${route} ${streetNumber}`.trim();
        onAddressExtractedRef.current(parsedAddress);
      }
    });

    return () => {
      listener.remove();
    };
  }, [placesLib]);

  return (
    <input
      ref={inputRef}
      type="text"
      className="crm-input"
      placeholder={placeholder}
      value={value || ""}
      onChange={(e) => onChange(e.target.value)}
      onKeyDown={(e) => {
        if (e.key === "Enter") e.preventDefault();
      }}
    />
  );
}

// Seção de endereço que utiliza o Autocomplete nos campos estratégicos
function AddressSection({
  title,
  address,
  onChange,
}: {
  title: string;
  address: AddressDto;
  onChange: (addr: AddressDto) => void;
}) {
  const handleChange = (field: keyof AddressDto, value: string) => {
    onChange({ ...address, [field]: value });
  };

  const handleExtracted = (extracted: AddressDto) => {
    // Mescla o endereço retornado pelo Google com os campos atuais para não apagar o que já foi digitado manualmente caso falte algum dado
    onChange({
      addressLine: extracted.addressLine || address.addressLine,
      neighborhood: extracted.neighborhood || address.neighborhood,
      city: extracted.city || address.city,
      state: extracted.state || address.state,
      postalCode: extracted.postalCode || address.postalCode,
      countryCode: extracted.countryCode || address.countryCode,
    });
  };

  return (
    <div style={{ marginTop: "16px", padding: "12px", border: "1px solid #e2e2e2", borderRadius: "6px" }}>
      <h4 style={{ margin: "0 0 12px 0", fontSize: "14px", color: "#333" }}>{title}</h4>

      <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "10px" }}>
        <div style={{ gridColumn: "1 / -1" }}>
          <label className="crm-label">Logradouro (Rua, Número)</label>
          <AutocompleteInput
            value={address.addressLine}
            onChange={(val) => handleChange("addressLine", val)}
            onAddressExtracted={handleExtracted}
            placeholder="Ex: Avenida Paulista, 1000..."
          />
        </div>

        <div>
          <label className="crm-label">Bairro</label>
          <input
            type="text"
            className="crm-input"
            value={address.neighborhood || ""}
            onChange={(e) => handleChange("neighborhood", e.target.value)}
          />
        </div>

        <div>
          <label className="crm-label">CEP</label>
          <AutocompleteInput
            value={address.postalCode}
            onChange={(val) => handleChange("postalCode", val)}
            onAddressExtracted={handleExtracted}
            placeholder="Ex: 01310-100"
          />
        </div>

        <div>
          <label className="crm-label">Cidade</label>
          <AutocompleteInput
            value={address.city}
            onChange={(val) => handleChange("city", val)}
            onAddressExtracted={handleExtracted}
            placeholder="Sua Cidade"
          />
        </div>

        <div>
          <label className="crm-label">Estado (UF)</label>
          <input
            type="text"
            className="crm-input"
            value={address.state || ""}
            onChange={(e) => handleChange("state", e.target.value)}
          />
        </div>

        <div style={{ gridColumn: "1 / -1" }}>
          <label className="crm-label">País</label>
          <input
            type="text"
            className="crm-input"
            value={address.countryCode || ""}
            onChange={(e) => handleChange("countryCode", e.target.value)}
          />
        </div>
      </div>
    </div>
  );
}

export function FiscalFormModal({
  customerId,
  initialData,
  onClose,
  onSuccess,
}: FiscalFormModalProps) {
  const [name, setName] = useState(initialData?.name || "");
  const [taxId, setTaxId] = useState(initialData?.taxId?.value || "");

  const [billingAddress, setBillingAddress] = useState<AddressDto>(
    initialData?.billingAddress || {}
  );
  const [shippingAddress, setShippingAddress] = useState<AddressDto>(
    initialData?.shippingAddress || {}
  );

  // Compara os JSONs no estado inicial para determinar se a flag deve iniciar marcada
  const isSameAddressInitially = 
    JSON.stringify(initialData?.billingAddress || {}) === JSON.stringify(initialData?.shippingAddress || {});
  
  const [isShippingDifferent, setIsShippingDifferent] = useState(
    initialData ? !isSameAddressInitially : false
  );

  const [isSubmitting, setIsSubmitting] = useState(false);
  const [errorMsg, setErrorMsg] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErrorMsg("");

    if (!name.trim()) {
      setErrorMsg("A Razão Social é obrigatória.");
      return;
    }

    setIsSubmitting(true);
    try {
      // Regra: se o usuário desmarcou a flag, espelha o billingAddress no shippingAddress
      const finalShippingAddress = isShippingDifferent ? shippingAddress : billingAddress;
      const normalizedTaxId = taxId.trim();

      const payload = {
        name: name.trim(),
        taxId: normalizedTaxId
          ? {
              value: normalizedTaxId,
              countryCode: (billingAddress.countryCode || "BR").trim().toUpperCase(),
            }
          : null,
        billingAddress,
        shippingAddress: finalShippingAddress,
      };

      if (initialData) {
        await updateFiscalEntity(initialData.id, payload);
        onClose();
        onSuccess("Ente Fiscal atualizado com sucesso.");
      } else {
        await addFiscalEntityToCustomer(customerId, payload);
        onClose();
        onSuccess("Ente Fiscal adicionado com sucesso.");
      }
    } catch (error) {
      console.error(error);
      setErrorMsg(
        initialData ? "Erro ao atualizar ente fiscal." : "Erro ao adicionar ente fiscal."
      );
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="crm-modal-overlay">
      <div className="crm-modal crm-modal-lg" style={{ maxHeight: "90vh", overflowY: "auto" }}>
        <div className="crm-modal-header">
          <h3 className="crm-modal-title">
            {initialData ? "Editar Ente Fiscal" : "Novo Ente Fiscal"}
          </h3>
          <button type="button" onClick={onClose} className="crm-btn-icon">
            <X size={18} />
          </button>
        </div>

        <APIProvider apiKey={API_KEY}>
          <form onSubmit={handleSubmit} className="crm-form">
            <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "10px" }}>
              <div>
                <label className="crm-label">Razão Social</label>
                <input
                  autoFocus
                  type="text"
                  className="crm-input"
                  placeholder="Razão Social"
                  value={name}
                  onChange={(e) => setName(e.target.value)}
                  required
                />
              </div>

              <div>
                <label className="crm-label">Documento Fiscal (CNPJ / CPF)</label>
                <input
                  type="text"
                  className="crm-input"
                  placeholder="CNPJ ou CPF"
                  value={taxId}
                  onChange={(e) => setTaxId(e.target.value)}
                />
              </div>
            </div>

            <AddressSection
              title="Endereço de Faturamento (Billing)"
              address={billingAddress}
              onChange={setBillingAddress}
            />

            <div style={{ marginTop: "16px", display: "flex", alignItems: "center", gap: "8px" }}>
              <input
                type="checkbox"
                id="diffShipping"
                checked={isShippingDifferent}
                onChange={(e) => setIsShippingDifferent(e.target.checked)}
                style={{ cursor: "pointer", width: "16px", height: "16px" }}
              />
              <label htmlFor="diffShipping" style={{ cursor: "pointer", fontSize: "13px", color: "#333", margin: 0, fontWeight: 500 }}>
                Endereço de entrega diferente do faturamento
              </label>
            </div>

            {isShippingDifferent && (
              <AddressSection
                title="Endereço de Entrega (Shipping)"
                address={shippingAddress}
                onChange={setShippingAddress}
              />
            )}

            {errorMsg && <div className="crm-error">{errorMsg}</div>}

            <button
              type="submit"
              disabled={isSubmitting}
              className="crm-btn-primary"
              style={{ marginTop: 16, width: "100%" }}
            >
              {isSubmitting ? "A gravar..." : "Salvar Ente Fiscal"}
            </button>
          </form>
        </APIProvider>
      </div>
    </div>
  );
}