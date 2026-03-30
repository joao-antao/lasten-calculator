# Copilot Instructions

## Project Overview
`lasten-calculator` is a simple tool to calculate Dutch municipal taxes (*gemeentelijke belastingen*) for a given household and property. Tax data is sourced from the **COELO** excel workbook (`Gemeentelijke_belastingen_2025.xlsx`).

## Architecture
Ports and adapters (hexagonal). Strict layer boundaries:
- **Domain** — entities, value objects, domain logic. No external dependencies.
- **Application** — command/query handlers, use case orchestration, port interfaces.
- **Infrastructure** — Cosmos DB, HTTP clients, Service Bus. Implements domain ports.
- **Console** — entry point, CLI parsing, user interaction.

# Hard Constraints
- All monetary values are `decimal`
- Never put business logic in infrastructure or API layers
- Never mock the system under test in tests
- Never use `#region` directives
- Always use dependency injection — never `new` up services manually
- Always use `async`/`await` for I/O — no `.Result` or `.Wait()`
- Always apply nullable reference types

## Documentation
- Document complex business logic and non-obvious decisions
- Keep README files current with setup instructions
- Create ADRs for significant architectural decisions

## Solution Structure
```
src/Lasten/
├── Lasten.sln
├── Lasten.Domain/          # Domain models (no external dependencies)
│   ├── Gemeente.cs                 – Municipality with tax rates and tariffs
│   └── GemeentelijkeBelastigen.cs  – Municipality tax calculation logic
├── Lasten.Infrastructure/  # External tools or systems
│   ├── GemeentenLoader.cs          – Reads COELO excel dataset
│   ├── Extensions/
│   │   └── XLCellExtensions.cs    
│   └── Coelo/
│       └── Gemeentelijke_belastingen_2025.xlsx
└── Lasten.Console/         # Console application
    └── Program.cs
```
