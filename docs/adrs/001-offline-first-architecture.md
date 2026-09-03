# ADR-001: Offline-First Architecture using Local Database

## Status
Accepted

## Date
2026-09-03

## Context
Field technicians operating the agricultural machinery CRM (MachineryCRM) frequently work in rural areas with degraded or non-existent internet connectivity. They require continuous access to machine history, parts catalogs, and the ability to register maintenance logs and draft quotes regardless of network status. A traditional cloud-dependent architecture would halt operations in these environments, rendering the application unusable at the point of service.

## Decision
Implement an offline-first architecture for the Progressive Web App (PWA). The frontend (React/TypeScript) will utilize a local client-side database (IndexedDB via a wrapper like Dexie.js, or SQLite via WebAssembly) as the primary data store for all immediate read and write operations. 

The system will employ a synchronization engine to reconcile local changes with the central PostgreSQL database (via the .NET REST API) when network connectivity is detected.

## Rationale
Treating the local database as the primary source of truth for the client ensures that the application remains fully functional in disconnected environments. 
*   **Availability:** Technicians can complete their workflows without network interruptions.
*   **Performance:** Local database queries execute with near-zero latency, significantly improving the user experience compared to relying on unstable cellular networks.
*   **Data Integrity:** The offline-first approach forces a robust synchronization contract between the React frontend and the .NET backend, ensuring that data is batched, validated, and safely stored upon reconnection.

## Consequences

### Positive
*   Guaranteed operational continuity for field technicians in rural zones.
*   Immediate UI response times, as data fetching and writing do not wait for network round-trips.
*   Reduced concurrent load on the central .NET backend, as data is synchronized asynchronously in batches rather than per interaction.

### Negative
*   Substantial increase in frontend architectural complexity. The client must handle data pre-fetching, local schema migrations, and sync state management.
*   Requirement for conflict resolution logic on the backend (e.g., handling concurrent modifications to the same machine record using a "last write wins" or timestamp-based merging strategy).
*   Risk of stale data (e.g., outdated parts pricing or inventory) if the device remains disconnected for extended periods. The application must explicitly display the "last synchronized" timestamp to mitigate business risks.