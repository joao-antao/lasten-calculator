# ADR: Hexagonal Architecture (Ports and Adapters)

📅 2026-04-08

## Context

The calculator needs to support multiple data sources (Excel files today, potentially APIs tomorrow) and multiple output surfaces (console today, potentially web or API later). We need clean boundaries to swap these without touching business logic.

## Decision

Adopt hexagonal architecture with strict layer separation:

| Layer | Project | Responsibility |
|---|---|---|
| **Domain** | `Lasten.Domain` | Tax entities, calculation logic. No external dependencies. |
| **Infrastructure** | `Lasten.Infrastructure` | Reads COELO Excel files. Implements domain ports. |
| **Console** | `Lasten.Console` | Entry point. CLI output. Wires up layers. |

### Layer rules

- Domain has **zero** external NuGet dependencies
- Infrastructure **never** contains business logic — only I/O and mapping
- Monetary values are always `decimal` — no `float`, no `double`
- All I/O is `async`/`await` — no `.Result` or `.Wait()`
- Nullable reference types are enabled project-wide

## Consequences

✅ Domain logic is independently testable  
✅ Data source can be swapped without touching domain or console  
⚠️ `Lasten.Console` currently references `Lasten.Infrastructure` directly — an application layer with use-case handlers is the intended intermediary (not yet introduced)

## Next steps

- Introduce `Lasten.Application` project with command/query handlers
- Wire infrastructure through application layer so console only depends on application contracts
