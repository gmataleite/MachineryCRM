import React, { useState, useEffect, useRef } from "react";
import { APIProvider, Map, AdvancedMarker, Pin, useMapsLibrary, useMap } from "@vis.gl/react-google-maps";
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

// Subcomponente conectado ao contexto do APIProvider para instanciar o Places Autocomplete
function PlaceAutocompleteInput({ onPlaceSelect }: { onPlaceSelect: (lat: number, lng: number) => void }) {
  const inputRef = useRef<HTMLInputElement>(null);
  const placesLib = useMapsLibrary("places");
  const map = useMap(); // Hook para controlar a câmera imperativamente apenas quando necessário

  // Mantém a referência atualizada do callback sem engatilhar re-execuções do useEffect
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

        // Move a câmera suavemente e aproxima o zoom apenas quando um endereço é escolhido no Autocomplete
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
      <MapPin size={15} color="#6E6C61" style={{ position: "absolute", left: 10, pointerEvents: "none" }} />
      <input
        ref={inputRef}
        type="text"
        placeholder="Digite um endereço, rodovia ou cidade (Autocomplete)..."
        onKeyDown={(e) => {
          if (e.key === "Enter") e.preventDefault();
        }}
        style={{
          padding: "9px 10px 9px 32px",
          border: "1px solid #DEDCD0",
          borderRadius: 4,
          width: "100%",
          fontSize: 13,
        }}
      />
    </div>
  );
}

export function GeoPointMapModal({ siteId, existingWaypointsCount, onClose, onSuccess }: GeoPointMapModalProps) {
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
        order: locationType === GeoLocationType.Waypoint ? existingWaypointsCount + 1 : undefined,
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
    <div
      style={{
        position: "fixed",
        top: 0,
        left: 0,
        right: 0,
        bottom: 0,
        background: "rgba(0,0,0,0.5)",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        zIndex: 1000,
      }}
    >
      <div
        style={{
          background: "#FFF",
          width: 560,
          borderRadius: 8,
          padding: 24,
          boxShadow: "0 4px 12px rgba(0,0,0,0.15)",
        }}
      >
        <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 16 }}>
          <h3 style={{ margin: 0, color: "#1F3B2C", fontSize: 16 }}>Novo Ponto Geográfico</h3>
          <button type="button" onClick={onClose} style={{ background: "none", border: "none", cursor: "pointer" }}>
            <X size={20} color="#6E6C61" />
          </button>
        </div>

        <APIProvider apiKey={API_KEY}>
          <form onSubmit={handleSubmit} style={{ display: "flex", flexDirection: "column", gap: 12 }}>
            <div style={{ display: "flex", gap: 10 }}>
              <input
                autoFocus
                type="text"
                placeholder="Descrição (Ex: Escritório, Entrada, Galpão)"
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                style={{ padding: "10px", border: "1px solid #DEDCD0", borderRadius: 4, flex: 1 }}
                required
              />
              <select
                value={locationType}
                onChange={(e) => setLocationType(Number(e.target.value) as GeoLocationType)}
                style={{ padding: "10px", border: "1px solid #DEDCD0", borderRadius: 4, width: 180, background: "#FFF" }}
              >
                <option value={GeoLocationType.Office}>Escritório</option>
                <option value={GeoLocationType.Waypoint}>Ponto de Rota (Entrada)</option>
                <option value={GeoLocationType.MachineLocation}>Local de Máquinas</option>
              </select>
            </div>

            {/* Campo integrado ao Google Places Autocomplete */}
            <PlaceAutocompleteInput
              onPlaceSelect={(lat, lng) => {
                setPosition({ lat, lng });
              }}
            />

            {/* Mapa Interativo com AdvancedMarkerElement */}
            <div style={{ height: 300, width: "100%", borderRadius: 4, overflow: "hidden", border: "1px solid #DEDCD0" }}>
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
                  <Pin background={"#1F3B2C"} glyphColor={"#CDE06E"} borderColor={"#1F3B2C"} />
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
              }}
            >
              <span>Arraste o pino ou clique no mapa para ajustar</span>
              <span>
                Lat: {position.lat.toFixed(5)} | Lng: {position.lng.toFixed(5)}
              </span>
            </div>

            {errorMsg && (
              <div style={{ color: "#DC2626", fontSize: 12.5, fontWeight: 600 }}>{errorMsg}</div>
            )}

            <button
              type="submit"
              disabled={isSubmitting}
              style={{
                background: "#1F3B2C",
                color: "#fff",
                border: "none",
                padding: "10px",
                borderRadius: 4,
                cursor: isSubmitting ? "not-allowed" : "pointer",
                fontWeight: 600,
                marginTop: 4,
              }}
            >
              {isSubmitting ? "A gravar..." : "Confirmar e Salvar Ponto"}
            </button>
          </form>
        </APIProvider>
      </div>
    </div>
  );
}