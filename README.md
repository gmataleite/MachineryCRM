# MachineryCRM

MachineryCRM is an offline-first Customer Relationship Management (CRM) system engineered specifically for agricultural machinery field services. It solves the critical business problem of executing and logging mechanical maintenance in rural areas with degraded or non-existent internet connectivity.

## Core Features
* **Offline-First Architecture:** Field technicians can log maintenance and parts usage without network connectivity using a React PWA backed by a local SQLite/IndexedDB database.
* **Bi-directional Synchronization:** Automated reconciliation of local maintenance logs with the central server upon connection restoration.
* **Geospatial Processing:** Integration with Google Maps Platform and PostGIS for equipment localization and logistics routing.
* **AI-Powered Insights:** A Python microservice leveraging Natural Language Processing (Text-to-SQL) and vector embeddings (pgvector) to query maintenance histories.
* **Role-Based Access Control (RBAC):** Segregated interfaces and data access for Technicians, Salespersons, and Managers.

## Technology Stack
| Layer | Technology |
| :--- | :--- |
| **Frontend** | React, TypeScript, Vite, PWA, Local Storage (SQLite/Dexie.js) |
| **Backend API** | C#, .NET Core, Entity Framework Core, Clean Architecture |
| **AI Microservice** | Python, LangChain/LlamaIndex |
| **Database** | PostgreSQL, PostGIS, pgvector |
| **Infrastructure** | Docker, GitHub Actions (CI/CD) |

## Documentation
The software development life cycle (SDLC) and architectural decisions are strictly documented within this repository using a Docs-as-Code approach.

* [Architecture Decision Records (ADRs)](./docs/adrs)
* [C4 Context & Container Models](./docs/architecture-c4.md)
* [Entity-Relationship Diagram (ERD)](./docs/erd.md)
* [Layer Topology](./docs/architecture-layers.md)
* [Synchronization Sequence Diagram](./docs/sequence-sync.md)
* [OpenAPI / Swagger Contract](./docs/api/openapi.yaml)

## Getting Started
*(Instructions for Docker infrastructure and local execution will be populated during Sprint 1).*
