# ADR-002: Frontend Technology Stack: React with TypeScript

## Status
Accepted

## Date
2026-09-03

## Context
The MachineryCRM requires a Progressive Web App (PWA) capable of operating in an offline-first capacity. This demands a robust client-side architecture to handle local database interactions (IndexedDB/SQLite), complex state management, and data synchronization. Furthermore, the technology choice must align with industry standards for modern web development to ensure maintainability and serve as a technically rigorous artifact for engineering portfolios.

## Decision
Adopt **React** as the UI library, strictly typed with **TypeScript**, and bundled via **Vite**.

## Rationale
*   **Performance and Payload:** React bundled with Vite generates highly optimized, minimal initial payloads. This is critical for rural environments with degraded connectivity, avoiding the heavy runtime downloads required by alternatives like Blazor WebAssembly.
*   **Type Safety and Contract Enforcement:** TypeScript provides structural typing that allows for seamless alignment with the C# backend. By utilizing OpenAPI generators, the frontend can automatically inherit DTOs and HTTP clients, ensuring a strict API contract.
*   **Ecosystem and Offline Capabilities:** The React/TypeScript ecosystem possesses the most mature libraries for PWA lifecycle management, local storage manipulation (e.g., Dexie.js, RxDB), and global state management required for offline synchronization queues.
*   **Market Alignment:** React and TypeScript represent the industry standard for full-stack and frontend engineering. Utilizing this stack demonstrates proficiency in managing complex Single Page Application (SPA) architectures, functional programming concepts, and unidirectional data flow.

## Consequences

### Positive
*   Minimized bundle sizes, leading to faster PWA installation and initialization times in the field.
*   End-to-end type safety between the .NET API and the client application via contract generation.
*   Access to a vast ecosystem of geospatial and routing libraries (essential for the Google Maps integration) without requiring complex interoperability bridges.

### Negative
*   Introduces a significant context switch for developers primarily accustomed to object-oriented backend languages (C#), requiring mastery of JavaScript's asynchronous nature and React's rendering lifecycle.
*   React is a library, not a framework; it requires manual selection and configuration of routing, state management, and data-fetching tools, increasing the initial architectural overhead.