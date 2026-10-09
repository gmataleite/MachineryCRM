import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import {
  DndContext,
  PointerSensor,
  useSensor,
  useSensors,
  useDraggable,
  useDroppable,
  type DragEndEvent,
} from "@dnd-kit/core";
import { CSS } from "@dnd-kit/utilities";
import {
  getCustomerById,
  updateCustomer,
  updateContact,
  deleteSite,
  deleteFiscalEntity,
  deleteContact,
  deleteGeoPoint, // <--- Nova importação
  reorderGeoPoints,
  GeoLocationType,
  type CustomerDto,
  type ContactDto,
  type GeoPointDto,
  type SiteDto,
  type FiscalEntityDto,
  type AddressDto, // <--- Nova importação
} from "../services/customerService";
import {
  Building2,
  MapPin,
  Landmark,
  ArrowLeft,
  Users,
  Phone,
  Mail,
  Pencil,
  Trash2,
  GripVertical,
  Briefcase,
  Signpost,
  Warehouse,
  FileText,
} from "lucide-react";
import { SiteFormModal } from "../components/SiteFormModal";
import { FiscalFormModal } from "../components/FiscalFormModal";
import { ContactFormModal } from "../components/ContactFormModal";
import { Toast, type ToastData } from "../components/Toast";
import { GeoPointMapModal } from "../components/GeoPointMapModal";

function GeoPointBadge({ type }: { type: GeoLocationType }) {
  const base: React.CSSProperties = {
    width: 22,
    height: 22,
    borderRadius: 4,
    display: "inline-flex",
    alignItems: "center",
    justifyContent: "center",
    flexShrink: 0,
  };

  if (type === GeoLocationType.Office) {
    return (
      <span title="Escritório (Office)" style={{ ...base, background: "#F5E6E4", color: "#9E3F33" }}>
        <Briefcase size={13} />
      </span>
    );
  }
  if (type === GeoLocationType.Waypoint) {
    return (
      <span title="Ponto de Passagem (Waypoint)" style={{ ...base, background: "#E4ECE6", color: "#2F5240" }}>
        <Signpost size={13} />
      </span>
    );
  }
  return (
    <span title="Local de Máquina (MachineLocation)" style={{ ...base, background: "#F7EBDC", color: "#B26B1F" }}>
      <Warehouse size={13} />
    </span>
  );
}

interface ContactDropTargetData {
  kind: "contact-drop";
  siteId: string | null;
  fiscalEntityId: string | null;
}

interface WaypointDragDropData {
  kind: "waypoint";
  siteId: string;
  geoPoint: GeoPointDto;
}

function ContactDropZone({
  id,
  siteId = null,
  fiscalEntityId = null,
  children,
}: {
  id: string;
  siteId?: string | null;
  fiscalEntityId?: string | null;
  children: React.ReactNode;
}) {
  const { isOver, active, setNodeRef } = useDroppable({
    id,
    data: {
      kind: "contact-drop",
      siteId,
      fiscalEntityId,
    } satisfies ContactDropTargetData,
  });

  const isContactDragging = active?.data.current?.kind === "contact";
  const highlight = isOver && isContactDragging;

  return (
    <div
      ref={setNodeRef}
      style={{
        borderRadius: 6,
        padding: highlight ? "6px" : "2px 0",
        background: highlight ? "#F4F8E6" : "transparent",
        border: highlight ? "1px dashed #94B03E" : "1px dashed transparent",
        transition: "all 0.15s ease",
        minHeight: 34,
      }}
    >
      {children}
    </div>
  );
}

function ContactRow({
  contact,
  onEdit,
  onDelete,
}: {
  contact: ContactDto;
  onEdit?: (contact: ContactDto) => void;
  onDelete?: (contact: ContactDto) => void;
}) {
  const { attributes, listeners, setNodeRef, transform, isDragging } = useDraggable({
    id: `contact-${contact.id}`,
    data: { kind: "contact", contact },
  });

  return (
    <div
      ref={setNodeRef}
      style={{
        background: "#F8F7F2",
        padding: "10px 12px",
        borderRadius: 4,
        marginBottom: 6,
        display: "flex",
        alignItems: "center",
        justifyContent: "space-between",
        gap: 10,
        transform: CSS.Translate.toString(transform),
        opacity: isDragging ? 0.65 : 1,
        boxShadow: isDragging ? "0 6px 16px rgba(0,0,0,0.14)" : "none",
        position: "relative",
        zIndex: isDragging ? 50 : 1,
      }}
    >
      <div style={{ display: "flex", alignItems: "center", gap: 10, textAlign: "left" }}>
        <span
          {...listeners}
          {...attributes}
          title="Arraste para mover este contato para outro card"
          style={{
            cursor: isDragging ? "grabbing" : "grab",
            display: "inline-flex",
            alignItems: "center",
            padding: "2px",
            touchAction: "none",
          }}
        >
          <GripVertical size={15} color="#9E9B8F" style={{ flexShrink: 0 }} />
        </span>

        <div>
          <div style={{ fontWeight: 700, fontSize: 13.5, color: "#23291F" }}>
            {contact.name}
          </div>
          {(contact.phone || contact.email) && (
            <div
              style={{
                display: "flex",
                alignItems: "center",
                gap: 14,
                color: "#6E6C61",
                fontSize: 12.5,
                marginTop: 3,
              }}
            >
              {contact.phone && (
                <span style={{ display: "inline-flex", alignItems: "center", gap: 4 }}>
                  <Phone size={12} color="#6E6C61" /> {contact.phone}
                </span>
              )}
              {contact.email && (
                <span style={{ display: "inline-flex", alignItems: "center", gap: 4 }}>
                  <Mail size={12} color="#6E6C61" /> {contact.email}
                </span>
              )}
            </div>
          )}
            <div
              style={{
                display: "flex",
                alignItems: "center",
                gap: 14,
                color: "#6E6C61",
                fontSize: 12.5,
                marginTop: 3,
              }}
            >
              {contact.observations && (
                <span style={{ display: "inline-flex", alignItems: "center", gap: 4 }}>
                  <FileText size={12} color="#6E6C61" /> {contact.observations}
                </span>
              )}
            </div>
        </div>
      </div>

      <div style={{ display: "flex", alignItems: "center", gap: 4, flexShrink: 0 }}>
        <button
          type="button"
          title="Editar contato"
          onClick={() => onEdit?.(contact)}
          className="crm-btn-icon"
        >
          <Pencil size={14} />
        </button>
        {onDelete && (
          <button
            type="button"
            title="Excluir contato"
            onClick={() => onDelete(contact)}
            className="crm-btn-icon"
          >
            <Trash2 size={14} />
          </button>
        )}
      </div>
    </div>
  );
}

function StaticGeoPointRow({
  geoPoint,
  onEdit,
  onDelete, // <--- Adicionado
}: {
  geoPoint: GeoPointDto;
  onEdit: (geoPoint: GeoPointDto) => void;
  onDelete: (geoPoint: GeoPointDto) => void; // <--- Adicionado
}) {
  return (
    <div
      style={{
        background: "#F8F7F2",
        padding: "8px 12px",
        borderRadius: 4,
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
        fontSize: 13.5,
      }}
    >
      <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
        <span style={{ width: 19, display: "inline-block" }} />
        <GeoPointBadge type={geoPoint.locationType} />
        <span style={{ color: "#23291F", fontWeight: 500 }}>{geoPoint.description}</span>
      </div>
      <div style={{ display: "flex", alignItems: "center", gap: 6 }}>
        <span className="crm-mono-id" style={{ fontSize: 12.5, color: "#6E6C61" }}>
          {geoPoint.latitude.toFixed(4)}, {geoPoint.longitude.toFixed(4)}
        </span>
        <button
          type="button"
          title="Editar ponto geográfico"
          onClick={() => onEdit(geoPoint)}
          className="crm-btn-icon"
        >
          <Pencil size={14} />
        </button>
        {/* Botão Excluir */}
        <button
          type="button"
          title="Excluir ponto geográfico"
          onClick={() => onDelete(geoPoint)}
          className="crm-btn-icon"
        >
          <Trash2 size={14} />
        </button>
      </div>
    </div>
  );
}

function WaypointSortableRow({
  siteId,
  geoPoint,
  sequenceIndex,
  onEdit,
  onDelete, // <--- Adicionado
}: {
  siteId: string;
  geoPoint: GeoPointDto;
  sequenceIndex: number;
  onEdit: (geoPoint: GeoPointDto) => void;
  onDelete: (geoPoint: GeoPointDto) => void; // <--- Adicionado
}) {
  const itemData: WaypointDragDropData = {
    kind: "waypoint",
    siteId,
    geoPoint,
  };

  const {
    attributes,
    listeners,
    setNodeRef: setDragRef,
    transform,
    isDragging,
  } = useDraggable({
    id: `waypoint-drag-${geoPoint.id}`,
    data: itemData,
  });

  const { isOver, active, setNodeRef: setDropRef } = useDroppable({
    id: `waypoint-drop-${geoPoint.id}`,
    data: itemData,
  });

  const isSameSiteWaypointOver =
    isOver &&
    active?.data.current?.kind === "waypoint" &&
    active?.data.current?.siteId === siteId &&
    active?.data.current?.geoPoint?.id !== geoPoint.id;

  const setCombinedRef = (node: HTMLDivElement | null) => {
    setDragRef(node);
    setDropRef(node);
  };

  return (
    <div
      ref={setCombinedRef}
      style={{
        background: isSameSiteWaypointOver ? "#F4F8E6" : "#F8F7F2",
        border: isSameSiteWaypointOver ? "1px dashed #94B03E" : "1px solid transparent",
        padding: "8px 12px",
        borderRadius: 4,
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
        fontSize: 13.5,
        transform: CSS.Translate.toString(transform),
        opacity: isDragging ? 0.65 : 1,
        boxShadow: isDragging ? "0 6px 16px rgba(0,0,0,0.14)" : "none",
        position: "relative",
        zIndex: isDragging ? 50 : 1,
        transition: isDragging ? "none" : "background 0.15s ease, border 0.15s ease",
      }}
    >
      <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
        <span
          {...listeners}
          {...attributes}
          title="Arraste para alterar a ordem deste ponto de passagem"
          style={{
            cursor: isDragging ? "grabbing" : "grab",
            display: "inline-flex",
            alignItems: "center",
            padding: "2px",
            touchAction: "none",
          }}
        >
          <GripVertical size={15} color="#9E9B8F" style={{ flexShrink: 0 }} />
        </span>

        <GeoPointBadge type={geoPoint.locationType} />

        <span
          style={{
            fontSize: 11.5,
            fontWeight: 700,
            color: "#2F5240",
            background: "#E4ECE6",
            padding: "1px 6px",
            borderRadius: 4,
            fontFamily: "monospace",
          }}
        >
          #{sequenceIndex}
        </span>

        <span style={{ color: "#23291F", fontWeight: 500 }}>{geoPoint.description}</span>
      </div>

      <div style={{ display: "flex", alignItems: "center", gap: 6 }}>
        <span className="crm-mono-id" style={{ fontSize: 12.5, color: "#6E6C61" }}>
          {geoPoint.latitude.toFixed(4)}, {geoPoint.longitude.toFixed(4)}
        </span>
        <button
          type="button"
          title="Editar ponto geográfico"
          onClick={() => onEdit(geoPoint)}
          className="crm-btn-icon"
        >
          <Pencil size={14} />
        </button>
        {/* Botão Excluir */}
        <button
          type="button"
          title="Excluir ponto geográfico"
          onClick={() => onDelete(geoPoint)}
          className="crm-btn-icon"
        >
          <Trash2 size={14} />
        </button>
      </div>
    </div>
  );
}

function sortSiteGeoPoints(geoPoints: GeoPointDto[] = []) {
  const offices = geoPoints.filter((gp) => gp.locationType === GeoLocationType.Office);
  const machines = geoPoints.filter((gp) => gp.locationType === GeoLocationType.MachineLocation);
  const waypoints = geoPoints
    .filter((gp) => gp.locationType === GeoLocationType.Waypoint)
    .sort((a, b) => (a.order ?? Number.MAX_SAFE_INTEGER) - (b.order ?? Number.MAX_SAFE_INTEGER));

  return { offices, machines, waypoints };
}

// Helper para formatar DTO de endereço em uma linha só, separada por vírgula
const formatAddress = (addr?: AddressDto | null) => {
  if (!addr) return "Não informado";
  const parts = [
    addr.addressLine,
    addr.neighborhood,
    addr.city,
    addr.state,
    addr.postalCode,
    addr.countryCode,
  ].filter(Boolean);
  return parts.length > 0 ? parts.join(", ") : "Não informado";
};

export function CustomerDetail() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [customer, setCustomer] = useState<CustomerDto | null>(null);
  const [isEditingCustomerName, setIsEditingCustomerName] = useState(false);
  const [customerNameDraft, setCustomerNameDraft] = useState("");
  const [isSavingCustomerName, setIsSavingCustomerName] = useState(false);
  const [loading, setLoading] = useState(true);
  const [toast, setToast] = useState<ToastData | null>(null);

  const sensors = useSensors(
    useSensor(PointerSensor, {
      activationConstraint: { distance: 5 },
    })
  );

  const handleDragEnd = async (event: DragEndEvent) => {
    const { active, over } = event;
    if (!over || !customer) return;

    const activeKind = active.data.current?.kind;

    if (activeKind === "contact") {
      const draggedContact = active.data.current?.contact as ContactDto | undefined;
      const target = over.data.current as ContactDropTargetData | undefined;
      if (!draggedContact || !target || target.kind !== "contact-drop") return;

      const currentSiteId = draggedContact.siteId ?? null;
      const currentFiscalId = draggedContact.fiscalEntityId ?? null;

      if (currentSiteId === target.siteId && currentFiscalId === target.fiscalEntityId) {
        return;
      }

      setCustomer((prev) => {
        if (!prev) return prev;
        return {
          ...prev,
          contacts: prev.contacts.map((c) =>
            c.id === draggedContact.id
              ? {
                  ...c,
                  siteId: target.siteId ?? undefined,
                  fiscalEntityId: target.fiscalEntityId ?? undefined,
                }
              : c
          ),
        };
      });

      try {
        await updateContact(draggedContact.id, {
          siteId: target.siteId ?? undefined,
          fiscalEntityId: target.fiscalEntityId ?? undefined,
          name: draggedContact.name,
          phone: draggedContact.phone,
          email: draggedContact.email,
          observations: draggedContact.observations,
        });
        setToast({ message: "Contato movido com sucesso.", type: "success" });
      } catch (err) {
        console.error("Erro ao mover contato:", err);
        setToast({ message: "Erro ao mover contato. Revertendo...", type: "error" });
        await fetchCustomerData();
      }
      return;
    }

    if (activeKind === "waypoint") {
      const sourceData = active.data.current as WaypointDragDropData | undefined;
      const targetData = over.data.current as WaypointDragDropData | undefined;

      if (
        !sourceData ||
        !targetData ||
        targetData.kind !== "waypoint" ||
        sourceData.siteId !== targetData.siteId ||
        sourceData.geoPoint.id === targetData.geoPoint.id
      ) {
        return;
      }

      const site = customer.sites.find((s) => s.id === sourceData.siteId);
      if (!site || !site.geoPoints) return;

      const { offices, machines, waypoints } = sortSiteGeoPoints(site.geoPoints);
      const oldIndex = waypoints.findIndex((w) => w.id === sourceData.geoPoint.id);
      const newIndex = waypoints.findIndex((w) => w.id === targetData.geoPoint.id);

      if (oldIndex === -1 || newIndex === -1) return;

      const reorderedWaypoints = [...waypoints];
      const [movedItem] = reorderedWaypoints.splice(oldIndex, 1);
      reorderedWaypoints.splice(newIndex, 0, movedItem);

      const updatedWaypoints = reorderedWaypoints.map((wp, idx) => ({
        ...wp,
        order: idx + 1,
      }));

      setCustomer((prev) => {
        if (!prev) return prev;
        return {
          ...prev,
          sites: prev.sites.map((s) =>
            s.id === site.id
              ? {
                  ...s,
                  geoPoints: [...offices, ...machines, ...updatedWaypoints],
                }
              : s
          ),
        };
      });

      try {
        const allPoints = [...offices, ...machines, ...updatedWaypoints];
        const reordePayLoad = allPoints.map((gp, index) => ({
          id: gp.id,
          order: index,
        }));

        await reorderGeoPoints(
          site.id,
          reordePayLoad
        );
        setToast({
          message: "Ordem dos pontos de passagem atualizada.",
          type: "success",
        });
      } catch (err) {
        console.error("Erro ao reordenar pontos geográficos:", err);
        setToast({
          message: "Erro ao atualizar ordem dos pontos. Revertendo...",
          type: "error",
        });
        await fetchCustomerData();
      }
    }
  };

  type ModalConfig =
    | { isOpen: false; type: null }
    | { isOpen: true; type: "site"; initialData?: SiteDto | null }
    | { isOpen: true; type: "fiscal"; initialData?: FiscalEntityDto | null }
    | {
        isOpen: true;
        type: "contact";
        siteId?: string | null;
        fiscalEntityId?: string | null;
        initialData?: ContactDto | null;
      }
    | {
        isOpen: true;
        type: "geopoint";
        siteId: string;
        initialData?: GeoPointDto | null;
      };

  const [modalConfig, setModalConfig] = useState<ModalConfig>({
    isOpen: false,
    type: null,
  });

  useEffect(() => {
    fetchCustomerData();
  }, [id]);

  const fetchCustomerData = async () => {
    if (!id) return;
    try {
      const data = await getCustomerById(id);
      setCustomer(data);
    } catch (err) {
      console.error("Erro", err);
    } finally {
      setLoading(false);
    }
  };

  const handleModalSuccess = (msg: string) => {
    setToast({ message: msg, type: "success" });
    void fetchCustomerData();
  };

  const closeModal = () => setModalConfig({ isOpen: false, type: null });

  if (loading) return <div className="crm-status">A carregar perfil...</div>;
  if (!customer) return <div className="crm-status">Cliente não encontrado.</div>;

  const generalContacts = customer.contacts.filter((c) => !c.siteId && !c.fiscalEntityId);

  const handleDeleteSite = async (site: SiteDto) => {
    if (!window.confirm(`Tem certeza que deseja remover o local produtivo "${site.name}"?`)) return;

    setCustomer((prev) => prev ? { ...prev, sites: prev.sites.filter(s => s.id !== site.id) } : prev);

    deleteSite(site.id)
    .then(() => {
      setToast({ message: "Local removido com sucesso.", type: "success" });
    }).catch(() => {
      setToast({ message: "Erro ao remover local.", type: "error" });
      fetchCustomerData();
    })
  };

  const handleDeleteFiscal = async (fiscal: FiscalEntityDto) => {
    if (!window.confirm(`Tem certeza que deseja remover o ente fiscal "${fiscal.name}"?`)) return;

    setCustomer((prev) => prev ? { ...prev, fiscalEntities: prev.fiscalEntities.filter(f => f.id !== fiscal.id) } : prev);

    deleteFiscalEntity(fiscal.id)
    .then(() => {
      setToast({ message: "Ente fiscal removido com sucesso.", type: "success" });
    }).catch(() => {
      setToast({ message: "Erro ao remover ente fiscal.", type: "error" });
      fetchCustomerData();
    })
  };

  const handleDeleteContact = async (contact: ContactDto) => {
    if (!window.confirm(`Tem certeza que deseja remover o contato "${contact.name}"?`)) return;
    
    setCustomer((prev) => prev ? { ...prev, contacts: prev.contacts.filter(c => c.id !== contact.id) } : prev);

    deleteContact(contact.id)
    .then(() => {
      setToast({ message: "Contato removido.", type: "success" });
    }).catch(() => {
      setToast({ message: "Erro ao remover contato.", type: "error" });
      fetchCustomerData();
    })
  };

  // <--- Lógica adicionada para remover pontos
  const handleDeleteGeoPoint = async (geoPoint: GeoPointDto) => {
    if (!window.confirm(`Tem certeza que deseja remover o ponto geográfico "${geoPoint.description}"?`)) return;

    setCustomer((prev) => {
      if (!prev) return prev;
      return {
        ...prev,
        sites: prev.sites.map((s) => ({
          ...s,
          geoPoints: s.geoPoints?.filter((gp) => gp.id !== geoPoint.id),
        })),
      };
    });

    deleteGeoPoint(geoPoint.id)
    .then(() => {
      setToast({ message: "Ponto geográfico removido com sucesso.", type: "success" });
    }).catch(() => {
      setToast({ message: "Erro ao remover ponto geográfico.", type: "error" });
      fetchCustomerData();
    });
  };

  const handleStartEditCustomerName = () => {
    setCustomerNameDraft(customer?.name ?? "");
    setIsEditingCustomerName(true);
  };

  const handleCancelEditCustomerName = () => {
    setCustomerNameDraft(customer?.name ?? "");
    setIsEditingCustomerName(false);
  };

  const handleSaveCustomerName = async () => {
    if (!customer) return;
    const updatedName = customerNameDraft.trim();

    if (!updatedName) {
      setToast({ message: "Informe um nome para o cliente.", type: "error" });
      return;
    }

    if (updatedName === customer.name) {
      setIsEditingCustomerName(false);
      return;
    }

    setIsSavingCustomerName(true);
    try {
      await updateCustomer(customer.id, { name: updatedName });
      setCustomer((prev) => (prev ? { ...prev, name: updatedName } : prev));
      setCustomerNameDraft(updatedName);
      setIsEditingCustomerName(false);
      setToast({ message: "Nome do cliente atualizado com sucesso.", type: "success" });
    } catch (err) {
      console.error("Erro ao atualizar nome do cliente:", err);
      setToast({ message: "Erro ao atualizar nome do cliente.", type: "error" });
    } finally {
      setIsSavingCustomerName(false);
    }
  };

  return (
    <div>
      <Toast toast={toast} onClose={() => setToast(null)} />

      {modalConfig.isOpen && modalConfig.type === "site" && (
        <SiteFormModal
          customerId={customer.id}
          initialData={modalConfig.initialData}
          onClose={closeModal}
          onSuccess={handleModalSuccess}
        />
      )}

      {modalConfig.isOpen && modalConfig.type === "fiscal" && (
        <FiscalFormModal
          customerId={customer.id}
          initialData={modalConfig.initialData}
          onClose={closeModal}
          onSuccess={handleModalSuccess}
        />
      )}

      {modalConfig.isOpen && modalConfig.type === "contact" && (
        <ContactFormModal
          customerId={customer.id}
          siteId={modalConfig.siteId}
          fiscalEntityId={modalConfig.fiscalEntityId}
          initialData={modalConfig.initialData}
          onClose={closeModal}
          onSuccess={handleModalSuccess}
        />
      )}

      {modalConfig.isOpen && modalConfig.type === "geopoint" && modalConfig.siteId && (
        <GeoPointMapModal
          siteId={modalConfig.siteId}
          initialData={modalConfig.initialData}
          existingWaypointsCount={
            customer.sites
              .find((s) => s.id === modalConfig.siteId)
              ?.geoPoints?.filter((gp) => gp.locationType === GeoLocationType.Waypoint).length || 0
          }
          onClose={closeModal}
          onSuccess={handleModalSuccess}
        />
      )}

      <button
        type="button"
        onClick={() => navigate("/customers")}
        className="crm-btn-back"
      >
        <ArrowLeft size={16} /> Voltar para lista
      </button>

      <DndContext onDragEnd={handleDragEnd} sensors={sensors}>
        <div className="crm-card-accent" style={{ marginBottom: 28 }}>
          <div style={{ display: "flex", alignItems: "center", gap: 8, marginBottom: 14 }}>
            <Building2 size={18} color="#1F3B2C" />
            <span
              style={{
                fontSize: 13,
                color: "#6E6C61",
                fontWeight: 700,
                letterSpacing: "0.04em",
              }}
            >
              CLIENTE
            </span>
            <span className="crm-mono-id" style={{ marginLeft: "auto" }}>
              {customer.id.split("-")[0].toUpperCase()}
            </span>
          </div>

          <div style={{ marginBottom: 18 }}>
            <label className="crm-label">Nome do cliente (grupo econômico)</label>
            <div
              style={{
                display: "flex",
                alignItems: "center",
                justifyContent: "space-between",
                border: "1px solid #DEDCD0",
                borderRadius: 4,
                padding: "9px 12px",
                background: "#FFFFFF",
              }}
            >
              {isEditingCustomerName ? (
                <input
                  type="text"
                  value={customerNameDraft}
                  onChange={(event) => setCustomerNameDraft(event.target.value)}
                  onKeyDown={(event) => {
                    if (event.key === "Enter") {
                      event.preventDefault();
                      void handleSaveCustomerName();
                    }
                    if (event.key === "Escape") {
                      event.preventDefault();
                      handleCancelEditCustomerName();
                    }
                  }}
                  className="crm-input"
                  style={{ marginRight: 8, paddingTop: 6, paddingBottom: 6 }}
                  disabled={isSavingCustomerName}
                  autoFocus
                />
              ) : (
                <span style={{ fontSize: 15, fontWeight: 500, color: "#23291F" }}>
                  {customer.name}
                </span>
              )}
              <div style={{ display: "flex", alignItems: "center", gap: 4 }}>
                {isEditingCustomerName ? (
                  <>
                    <button
                      type="button"
                      onClick={handleCancelEditCustomerName}
                      className="crm-btn-outline"
                      style={{ padding: "5px 8px" }}
                      disabled={isSavingCustomerName}
                    >
                      Cancelar
                    </button>
                    <button
                      type="button"
                      onClick={() => void handleSaveCustomerName()}
                      className="crm-btn-primary"
                      style={{ padding: "5px 8px", fontSize: 12.5 }}
                      disabled={isSavingCustomerName}
                    >
                      Salvar
                    </button>
                  </>
                ) : (
                  <button
                    type="button"
                    title="Editar cliente"
                    className="crm-btn-icon"
                    onClick={handleStartEditCustomerName}
                  >
                    <Pencil size={14} />
                  </button>
                )}
              </div>
            </div>
          </div>

          <div className="crm-card-divider" style={{ marginTop: 0 }}>
            <div
              style={{
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center",
                marginBottom: 10,
              }}
            >
              <div style={{ display: "flex", alignItems: "center", gap: 6, color: "#6E6C61" }}>
                <Users size={14} />
                <span style={{ fontSize: 13, fontWeight: 600 }}>Contatos gerais do cliente</span>
              </div>
              <button
                type="button"
                onClick={() => setModalConfig({ isOpen: true, type: "contact" })}
                className="crm-btn-outline"
              >
                + Contato
              </button>
            </div>
            <ContactDropZone id="drop-general" siteId={null} fiscalEntityId={null}>
              {generalContacts.length === 0 ? (
                <div className="crm-empty-hint">
                  Nenhum contato cadastrado. Arraste um contato para cá, ou:
                </div>
              ) : (
                generalContacts.map((c) => (
                  <ContactRow
                    key={c.id}
                    contact={c}
                    onEdit={(contact) =>
                      setModalConfig({ isOpen: true, type: "contact", initialData: contact })
                    }
                    onDelete={handleDeleteContact}
                  />
                ))
              )}
            </ContactDropZone>
          </div>
        </div>

        <div className="crm-section-header">
          <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
            <MapPin size={18} color="#1F3B2C" />
            <h2 className="crm-section-title">Locais produtivos</h2>
          </div>
          <button
            type="button"
            onClick={() => setModalConfig({ isOpen: true, type: "site" })}
            className="crm-btn-primary"
            style={{ padding: "7px 14px", fontSize: 13 }}
          >
            + Novo local
          </button>
        </div>

        {customer.sites.map((site) => {
          const siteContacts = customer.contacts.filter((c) => c.siteId === site.id);
          
          const locationLine = [
            site.address?.city,
            site.address?.state,
            site.address?.countryCode,
          ]
            .filter(Boolean)
            .join(" - ");
          
          const { offices, machines, waypoints } = sortSiteGeoPoints(site.geoPoints);
          const hasGeoPoints =
            offices.length > 0 || machines.length > 0 || waypoints.length > 0;

          return (
            <div key={site.id} className="crm-card">
              <div
                style={{
                  display: "flex",
                  justifyContent: "space-between",
                  alignItems: "flex-start",
                }}
              >
                <div>
                  <div style={{ fontWeight: 700, fontSize: 16, color: "#23291F" }}>
                    {site.name}
                  </div>
                  {locationLine && (
                    <div style={{ fontSize: 13.5, color: "#6E6C61", marginTop: 3 }}>
                      {locationLine}
                    </div>
                  )}
                  {site.observations && (
                    <div
                      style={{
                        fontSize: 13,
                        color: "#6E6C61",
                        fontStyle: "italic",
                        marginTop: 4,
                      }}
                    >
                      {site.observations}
                    </div>
                  )}
                </div>

                <div style={{ display: "flex", alignItems: "center", gap: 4 }}>
                  <button type="button" onClick={() => handleDeleteSite(site)} title="Excluir local" className="crm-btn-icon">
                    <Trash2 size={15} />
                  </button>
                  <button type="button" onClick={() => setModalConfig({ isOpen: true, type: "site", initialData: site })}title="Editar local" className="crm-btn-icon">
                    <Pencil size={15} />
                  </button>
                </div>
              </div>

              <div className="crm-card-divider">
                <div
                  style={{
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "center",
                    marginBottom: 10,
                  }}
                >
                  <div style={{ display: "flex", alignItems: "center", gap: 6, color: "#6E6C61" }}>
                    <Users size={14} />
                    <span style={{ fontSize: 13, fontWeight: 600 }}>Contatos deste local</span>
                  </div>
                  <button
                    type="button"
                    onClick={() =>
                      setModalConfig({ isOpen: true, type: "contact", siteId: site.id })
                    }
                    className="crm-btn-outline"
                  >
                    + Contato
                  </button>
                </div>
                <ContactDropZone id={`drop-site-${site.id}`} siteId={site.id} fiscalEntityId={null}>
                  {siteContacts.length === 0 ? (
                    <div className="crm-empty-hint">
                      Nenhum contato cadastrado. Arraste um contato para cá, ou:
                    </div>
                  ) : (
                    siteContacts.map((c) => (
                      <ContactRow
                        key={c.id}
                        contact={c}
                        onEdit={(contact) =>
                          setModalConfig({
                            isOpen: true,
                            type: "contact",
                            siteId: site.id,
                            initialData: contact,
                          })
                        }
                        onDelete={handleDeleteContact}
                      />
                    ))
                  )}
                </ContactDropZone>
              </div>

              <div className="crm-card-divider">
                <div
                  style={{
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "center",
                    marginBottom: 10,
                  }}
                >
                  <div style={{ display: "flex", alignItems: "center", gap: 6, color: "#6E6C61" }}>
                    <MapPin size={14} />
                    <span style={{ fontSize: 13, fontWeight: 600 }}>Pontos geográficos</span>
                  </div>
                  <button
                    type="button"
                    onClick={() =>
                      setModalConfig({ isOpen: true, type: "geopoint", siteId: site.id })
                    }
                    className="crm-btn-outline"
                  >
                    + Ponto
                  </button>
                </div>

                {!hasGeoPoints ? (
                  <div className="crm-empty-hint">Nenhum ponto geográfico cadastrado.</div>
                ) : (
                  <div style={{ display: "flex", flexDirection: "column", gap: 6 }}>
                    {offices.map((gp) => (
                      <StaticGeoPointRow
                        key={gp.id}
                        geoPoint={gp}
                        onEdit={(geoPoint) =>
                          setModalConfig({
                            isOpen: true,
                            type: "geopoint",
                            siteId: site.id,
                            initialData: geoPoint,
                          })
                        }
                        onDelete={handleDeleteGeoPoint} // <--- Passando o Handler
                      />
                    ))}

                    {machines.map((gp) => (
                      <StaticGeoPointRow
                        key={gp.id}
                        geoPoint={gp}
                        onEdit={(geoPoint) =>
                          setModalConfig({
                            isOpen: true,
                            type: "geopoint",
                            siteId: site.id,
                            initialData: geoPoint,
                          })
                        }
                        onDelete={handleDeleteGeoPoint} // <--- Passando o Handler
                      />
                    ))}

                    {waypoints.map((gp, idx) => (
                      <WaypointSortableRow
                        key={gp.id}
                        siteId={site.id}
                        geoPoint={gp}
                        sequenceIndex={idx + 1}
                        onEdit={(geoPoint) =>
                          setModalConfig({
                            isOpen: true,
                            type: "geopoint",
                            siteId: site.id,
                            initialData: geoPoint,
                          })
                        }
                        onDelete={handleDeleteGeoPoint} // <--- Passando o Handler
                      />
                    ))}
                  </div>
                )}
              </div>
            </div>
          );
        })}

        <div className="crm-section-header" style={{ marginTop: 32 }}>
          <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
            <Landmark size={18} color="#1F3B2C" />
            <h2 className="crm-section-title">Entes fiscais</h2>
          </div>
          <button
            type="button"
            onClick={() => setModalConfig({ isOpen: true, type: "fiscal" })}
            className="crm-btn-primary"
            style={{ padding: "7px 14px", fontSize: 13 }}
          >
            + Novo fiscal
          </button>
        </div>

        {customer.fiscalEntities.map((fiscal) => {
          const fiscalContacts = customer.contacts.filter((c) => c.fiscalEntityId === fiscal.id);
          
          // Compatibilidade atualizada com a nova prop taxId
          const docText = fiscal.taxId?.value ? `Documento: ${fiscal.taxId.value}` : "";
          const docLine = [docText].filter(Boolean).join(" · ");

          return (
            <div key={fiscal.id} className="crm-card">
              <div
                style={{
                  display: "flex",
                  justifyContent: "space-between",
                  alignItems: "flex-start",
                }}
              >
                <div>
                  <div style={{ fontWeight: 700, fontSize: 16, color: "#23291F" }}>
                    {fiscal.name}
                  </div>
                  {docLine && (
                    <div style={{ fontSize: 13.5, color: "#6E6C61", marginTop: 3 }}>
                      {docLine}
                    </div>
                  )}
                  
                  {/* Endereço de Faturamento e Entrega mapeados do novo Helper */}
                  <div style={{ fontSize: 13, color: "#6E6C61", marginTop: 8 }}>
                    <span style={{ fontWeight: 600 }}>Endereço de faturamento:</span> {formatAddress(fiscal.billingAddress)}
                  </div>
                  <div style={{ fontSize: 13, color: "#6E6C61", marginTop: 2 }}>
                    <span style={{ fontWeight: 600 }}>Endereço de entrega:</span> {formatAddress(fiscal.shippingAddress)}
                  </div>
                </div>

                <div style={{ display: "flex", alignItems: "center", gap: 4 }}>
                  <button type="button" onClick={() => handleDeleteFiscal(fiscal)} title="Excluir ente fiscal" className="crm-btn-icon">
                    <Trash2 size={15} />
                  </button>
                  <button type="button" onClick={() => setModalConfig({ isOpen: true, type: "fiscal", initialData: fiscal })} title="Editar ente fiscal" className="crm-btn-icon">
                    <Pencil size={15} />
                  </button>
                </div>
              </div>

              <div className="crm-card-divider">
                <div
                  style={{
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "center",
                    marginBottom: 10,
                  }}
                >
                  <div style={{ display: "flex", alignItems: "center", gap: 6, color: "#6E6C61" }}>
                    <Users size={14} />
                    <span style={{ fontSize: 13, fontWeight: 600 }}>Contatos deste fiscal</span>
                  </div>
                  <button
                    type="button"
                    onClick={() =>
                      setModalConfig({
                        isOpen: true,
                        type: "contact",
                        fiscalEntityId: fiscal.id,
                      })
                    }
                    className="crm-btn-outline"
                  >
                    + Contato
                  </button>
                </div>
                <ContactDropZone
                  id={`drop-fiscal-${fiscal.id}`}
                  siteId={null}
                  fiscalEntityId={fiscal.id}
                >
                  {fiscalContacts.length === 0 ? (
                    <div className="crm-empty-hint">
                      Nenhum contato cadastrado. Arraste um contato para cá, ou:
                    </div>
                  ) : (
                    fiscalContacts.map((c) => (
                      <ContactRow
                        key={c.id}
                        contact={c}
                        onEdit={(contact) =>
                          setModalConfig({
                            isOpen: true,
                            type: "contact",
                            fiscalEntityId: fiscal.id,
                            initialData: contact,
                          })
                        }
                        onDelete={handleDeleteContact}
                      />
                    ))
                  )}
                </ContactDropZone>
              </div>
            </div>
          );
        })}
      </DndContext>
    </div>
  );
}