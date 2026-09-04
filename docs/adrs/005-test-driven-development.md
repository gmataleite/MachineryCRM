# ADR-005: Test-Driven Development (TDD) as Core Methodology

## Status
Accepted

## Date
2026-09-03

## Context
MachineryCRM processes critical operational data, including offline maintenance logs, synchronization conflict resolution, and geospatial routing. Regressions in these areas cause immediate operational failure in the field. Developing these complex features without a strict testing methodology leads to brittle code, tight coupling, and extensive manual testing overhead.

## Decision
Adopt **Test-Driven Development (TDD)** as the mandatory engineering methodology for the entire system lifecycle. No production code will be written without a failing test (Red), followed by the minimal code to pass the test (Green), and subsequent optimization (Refactor).

*   **Backend (.NET):** xUnit for test execution, NSubstitute for mocking, and FluentAssertions for validation. Integration tests will execute against a Dockerized PostgreSQL instance.
*   **Frontend (React):** Vitest (or Jest) as the test runner, React Testing Library for component behavior testing, and Mock Service Worker (MSW) for API interception.

## Rationale
*   **Design Feedback:** TDD forces decoupled, highly cohesive code. If a class is difficult to test, its design is flawed, enforcing the principles of Clean Architecture.
*   **Refactoring Confidence:** A comprehensive test suite allows safe modification of complex algorithms (e.g., the bidirectional offline sync logic) without fear of introducing hidden regressions.
*   **Living Documentation:** The test suite serves as the most accurate, executable documentation of the system's business rules.

## Consequences

### Positive
*   Drastic reduction in post-deployment bugs and logical errors.
*   Enforces strict adherence to the Dependency Inversion Principle (SOLID).
*   High code coverage is achieved organically, not as an afterthought.

### Negative
*   Increases the initial development time for each feature.
*   Requires rigorous discipline and domain knowledge to write valuable behavioral tests rather than superficial implementation tests.