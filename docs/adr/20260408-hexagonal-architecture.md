# ADR: Hexagonal Architecture (Ports and Adapters)

📅 2026-04-08

## Context

The calculator needs to support multiple data sources (Excel files today, potentially APIs tomorrow) and multiple output surfaces (console today, potentially web or API later). We need clean boundaries to swap these without touching business logic.

## Decision

Adopt hexagonal architecture with strict layer separation:

| Layer              | Project                 | Responsibility                                                                |
|--------------------|-------------------------|-------------------------------------------------------------------------------|
| **Domain**         | `Lasten.Domain`         | Tax entities, calculation logic. No external dependencies.                    |
| **Application**    | `Lasten.Application`    | Use-case handlers, inbound ports (queries/results), outbound port interfaces. |
| **Infrastructure** | `Lasten.Infrastructure` | Reads COELO Excel files. Implements application port interfaces.              |
| **Console**        | `Lasten.Console`        | Entry point. CLI output. Wires up layers.                                     |

### Layer rules

- Domain has **zero** external dependencies
- Application depends only on domain; no I/O, no infrastructure references
- Infrastructure **never** contains business logic, only I/O and mapping
- Monetary values are always `decimal` — no `float`, no `double`
- All I/O is `async`/`await` — no `.Result` or `.Wait()`
- Nullable reference types are enabled project-wide

## Consequences

✅ Domain logic is independently testable  
✅ Application use cases are testable without real I/O (port interfaces can be mocked)  
✅ Data source can be swapped without touching domain, application, or console
