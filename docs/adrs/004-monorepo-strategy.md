# ADR-004: Repository Strategy: Monorepo Setup

## Status
Accepted

## Date
2026-09-03

## Context
MachineryCRM is a heterogeneous system composed of multiple distinct technological stacks: a React/TypeScript Progressive Web App, a .NET C# REST API, a Python-based AI microservice, and shared infrastructure configurations (Docker, PostgreSQL scripts). Managing these components across multiple isolated repositories (polyrepo) introduces significant overhead in version synchronization, end-to-end testing, and cross-team communication, particularly when API contracts change.

## Decision
Adopt a **Monorepo (Monolithic Repository)** strategy. The entire source code for the platform will be housed within a single Git repository, logically partitioned into distinct directories (`/src/frontend`, `/src/backend`, `/src/ai-service`, `/infra`, `/docs`).

## Rationale
*   **Atomic Commits:** A feature that requires changes across the database schema, the backend API, and the frontend UI can be encapsulated in a single, atomic commit and Pull Request. This ensures that the `main` branch always represents a fully cohesive and compatible state of the system.
*   **Unified CI/CD:** A single GitHub Actions pipeline can easily orchestrate end-to-end testing by spinning up all services via `docker-compose` without needing to pull from multiple external repositories or manage complex webhook triggers.
*   **Contract Sharing:** Keeping the OpenAPI specification (`openapi.yaml`) in the same repository as both the API that implements it and the frontend that consumes it guarantees that code generation tools are always referencing the correct, up-to-date contract.
*   **Developer Experience:** A single `git clone` provides the developer with the entire ecosystem required to run the application locally.

## Consequences

### Positive
*   Eliminates version skew and dependency hell between internal project components.
*   Simplifies code reviews by providing complete context of a feature's impact across the entire stack in one Pull Request.
*   Centralizes documentation (`/docs`) alongside the code, making it easier to keep architecture diagrams and ADRs synchronized with the actual implementation.

### Negative
*   The repository size will grow faster than isolated repositories, downloading full history for all stacks even if a developer only works on one.
*   CI/CD pipelines require conditional logic (e.g., using `paths` filters in GitHub Actions) to avoid rebuilding the .NET backend if only a Markdown file or a React component was modified.