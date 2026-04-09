# Copilot Instructions

## Project Overview
`lasten` is a simple tool to calculate Dutch municipal taxes (*gemeentelijke belastingen*) for a given household and property. 

## Architecture
Ports and adapters (hexagonal). Strict layer boundaries:
- **Domain** — entities, value objects, domain logic. No external dependencies.
- **Application** — command/query handlers, use case orchestration, ports (technology-agnostic entry or exit points to the application).
- **Infrastructure** — connect to external systems by implementing adapters (conforming to ports).
- **Console** — entry point, CLI parsing, user interaction.

# Hard Constraints
- Never put domain logic outside the domain layer
- Never mock the system under test in tests
- Never use `#region` directives
- Always apply nullable reference types

## Documentation
- Document complex business logic and non-obvious decisions
- Keep README files current with setup instructions
- Create ADRs for significant architectural decisions

## Solution Structure
```
src/
├── Lasten.sln
├── Lasten.Domain/                  # Domain models (no external dependencies)
│   ├── Gemeentelijkebelastingen/
│   │   ├── Gemeente.cs                     – Municipality with tax rates and tariffs
│   │   └── GemeentelijkeBelastigen.cs      – Municipality tax calculation logic
│   └── Waterschapsbelastingen/
│       ├── Waterschap.cs                   – Water authority with tax rates
│       └── WaterschapBelastingen.cs        – Water authority tax calculation logic
├── Lasten.Application/             # Use cases and ports
│   ├── Result.cs
│   ├── Belasting/
│   │   ├── BerekenBelastingQuery.cs
│   │   ├── BerekenBelastingResult.cs
│   │   └── BerekenBelastingUseCase.cs
│   └── Ports/
│       ├── IGemeenteRepository.cs
│       ├── IGemeenteWaterschapMapping.cs
│       └── IWaterschappenRepository.cs
├── Lasten.Infrastructure/          # Adapters for external systems
│   ├── GemeentenLoader.cs              – Reads COELO excel dataset
│   ├── GemeenteRepository.cs
│   ├── GemeenteWaterschapMapping.cs
│   ├── WaterschapLoader.cs
│   ├── WaterschappenRepository.cs
│   ├── Extensions/
│   │   └── XLCellExtensions.cs
│   └── Coelo/
│       └── gemeente_waterschap_2025.json
└── Lasten.Console/                 # Console application entry point
    └── Program.cs
tests/
├── Lasten.Domain.UnitTests/
└── Lasten.Application.UnitTests/
```