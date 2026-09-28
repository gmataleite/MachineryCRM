import React, { useState, useEffect, useRef } from "react";
import {
  APIProvider,
  Map,
  AdvancedMarker,
  Pin,
  useMapsLibrary,
  useMap,
} from "@vis.gl/react-google-maps";
import { addGeoPointToSite, GeoLocationType } from "../services/customerService";
import { X, MapPin } from "lucide-react";

interface GeoPointMapModalProps {
  siteId: string;
  existingWaypointsCount: number;
  onClose: () => void;
  onSuccess: (msg: string) => void;
}

const API_KEY = import.meta.env.VITE_GOOGLE_MAPS_API_KEY || "";
const MAP_ID = import.meta.env.VITE_GOOGLE_MAPS_MAP_ID || "DEMO_MAP_ID";

function PlaceAutocompleteInput({
  onPlaceSelect,
}: {
  onPlaceSelect: (lat: number, lng: number) => void;
}) {
  const inputRef = useRef<HTMLInputElement>(null);
  const placesLib = useMapsLibrary("places");
  const map = useMap();

  const onPlaceSelectRef = useRef(onPlaceSelect);
  onPlaceSelectRef.current = onPlaceSelect;

  useEffect(() => {
    if (!placesLib || !inputRef.current) return;

    const autocomplete = new placesLib.Autocomplete(inputRef.current, {
      fields: ["geometry", "name", "formatted_address"],
      componentRestrictions: { country: "br" },
    });

    const listener = autocomplete.addListener("place_changed", () => {
      const place = autocomplete.getPlace();
      if (place.geometry?.location) {
        const lat = place.geometry.location.lat();
        const lng = place.geometry.location.lng();

        onPlaceSelectRef.current(lat, lng);

        if (map) {
          map.panTo({ lat, lng });
          map.setZoom(17);
        }
      }
    });

    return () => {
      listener.remove();
    };
  }, [placesLib, map]);

  return (
    <div style={{ position: "relative", display: "flex", alignItems: "center" }}>
      <MapPin
        size={15}
        color="#6E6C61"
        style={{ position: "absolute", left: 10, pointerEvents: "none" }}
      />
      <input
        ref={inputRef}
        type="text"
        className="crm-input"
        placeholder="Digite um endereço, rodovia ou cidade (Autocomplete)..."
        onKeyDown={(e) => {
          if (e.key === "Enter") e.preventDefault();
        }}
        style={{ paddingLeft: 32, fontSize: 13 }}
      />
    </div>
  );
}

export function GeoPointMapModal({
  siteId,
  existingWaypointsCount,
  onClose,
  onSuccess,
}: GeoPointMapModalProps) {
  const [description, setDescription] = useState("");
  const [locationType, setLocationType] = useState<GeoLocationType>(GeoLocationType.Office);
  const [position, setPosition] = useState<{ lat: number; lng: number }>({
    lat: -22.3145,
    lng: -49.0587,
  });

  const [isSubmitting, setIsSubmitting] = useState(false);
  const [errorMsg, setErrorMsg] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErrorMsg("");

    if (!description.trim()) {
      setErrorMsg("A descrição do ponto é obrigatória.");
      return;
    }

    setIsSubmitting(true);
    try {
      await addGeoPointToSite(siteId, {
        description: description.trim(),
        latitude: Number(position.lat.toFixed(6)),
        longitude: Number(position.lng.toFixed(6)),
        locationType,
        order:
          locationType === GeoLocationType.Waypoint ? existingWaypointsCount + 1 : undefined,
      });
      onSuccess("Ponto geográfico adicionado com sucesso.");
      onClose();
    } catch (error) {
      console.error(error);
      setErrorMsg("Falha ao salvar o ponto geográfico no servidor.");
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="crm-modal-overlay">
      <div className="crm-modal crm-modal-lg">
        <div className="crm-modal-header">
          <h3 className="crm-modal-title">Novo Ponto Geográfico</h3>
          <button type="button" onClick={onClose} className="crm-btn-icon">
            <X size={18} />
          </button>
        </div>

        <APIProvider apiKey={API_KEY}>
          <form onSubmit={handleSubmit} className="crm-form">
            <div style={{ display: "flex", gap: 10 }}>
              <div style={{ flex: 1 }}>
                <label className="crm-label">Descrição</label>
                <input
                  autoFocus
                  type="text"
                  className="crm-input"
                  placeholder="Ex: Escritório, Entrada, Galpão"
                  value={description}
                  onChange={(e) => setDescription(e.target.value)}
                  required
                />
              </div>

              <div style={{ width: 190 }}>
                <label className="crm-label">Tipo de Ponto</label>
                <select
                  value={locationType}
                  onChange={(e) => setLocationType(Number(e.target.value) as GeoLocationType)}
                  className="crm-input"
                >
                  <option value={GeoLocationType.Office}>Escritório</option>
                  <option value={GeoLocationType.Waypoint}>Ponto de Rota (Entrada)</option>
                  <option value={GeoLocationType.MachineLocation}>Local de Máquinas</option>
                </select>
              </div>
            </div>

            <div>
              <label className="crm-label">Pesquisar Endereço</label>
              <PlaceAutocompleteInput
                onPlaceSelect={(lat, lng) => {
                  setPosition({ lat, lng });
                }}
              />
            </div>

            <div
              style={{
                height: 300,
                width: "100%",
                borderRadius: 4,
                overflow: "hidden",
                border: "1px solid #DEDCD0",
              }}
            >
              <Map
                mapId={MAP_ID}
                defaultCenter={position}
                defaultZoom={15}
                gestureHandling={"greedy"}
                onClick={(e) => {
                  if (e.detail.latLng) {
                    setPosition({ lat: e.detail.latLng.lat, lng: e.detail.latLng.lng });
                  }
                }}
              >
                <AdvancedMarker
                  position={position}
                  draggable={true}
                  onDragEnd={(e) => {
                    if (e.latLng) {
                      setPosition({ lat: e.latLng.lat(), lng: e.latLng.lng() });
                    }
                  }}
                >
                  <Pin
                    background={"#1F3B2C"}
                    glyphColor={"#CDE06E"}
                    borderColor={"#1F3B2C"}
                  />
                </AdvancedMarker>
              </Map>
            </div>

            <div
              style={{
                display: "flex",
                justifyContent: "space-between",
                fontSize: 12,
                color: "#6E6C61",
                fontFamily: "monospace",
                background: "#F8F7F2",
                padding: "8px 12px",
                borderRadius: 4,
                border: "1px solid #EAE8DD",
              }}
            >
              <span>Arraste o pino ou clique no mapa para ajustar</span>
              <span>
                Lat: {position.lat.toFixed(5)} | Lng: {position.lng.toFixed(5)}
              </span>
            </div>

            {errorMsg && <div className="crm-error">{errorMsg}</div>}

            <button
              type="submit"
              disabled={isSubmitting}
              className="crm-btn-primary"
              style={{ marginTop: 4 }}
            >
              {isSubmitting ? "A gravar..." : "Confirmar e Salvar Ponto"}
            </button>
          </form>
        </APIProvider>
      </div>
    </div>
  );
}