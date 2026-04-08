# Sprint 1

📅 2026-04-08

## Goal

- [x] Introduce `Lasten.Application` with command/query handlers to decouple console from infrastructure
- [x] Build a municipality → water authority mapping (342 gemeenten → 21 waterschappen)

---

## What was built

### Application — `Lasten.Application`

| Type                         | Description                                                                               |
|------------------------------|-------------------------------------------------------------------------------------------|
| `IGemeentenRepository`       | Port interface for municipality data access                                               |
| `IWaterschappenRepository`   | Port interface for water authority data access                                            |
| `IGemeenteWaterschapMapping` | Port interface for resolving gemeente → waterschap; returns `null` when no mapping exists |
| `BerekenLastenQuery`         | Input record: gemeente name, WOZ value, household type, ownership flag                    |
| `BerekenLastenResult`        | Output records with computed `Total`; `WaterschapLasten` is nullable                      |
| `BerekenLastenHandler`       | Orchestrates domain calculation; the only place that constructs domain objects            |

### Infrastructure — `Lasten.Infrastructure`

| Component                       | Description                                                                                                   |
|---------------------------------|---------------------------------------------------------------------------------------------------------------|
| `GemeenteRepository`            | `IGemeenteRepository` adapter wrapping existing `GemeentenLoader`;                                            |
| `WaterschappenRepository`       | `IWaterschappenRepository` adapter wrapping existing `WaterschapLoader`;                                      |
| `GemeenteWaterschapMapping`     | `IGemeenteWaterschapMapping` adapter; loads `gemeente_waterschap_2025.json` at startup via `System.Text.Json` |
| `gemeente_waterschap_2025.json` | 342 gemeente codes mapped to 21 waterschap codes (COELO); __done by an LLM and should require validation__    |

### Console — `Lasten.Console`

Uses application use case to compute and display results for a hardcoded input (Leiden, €511,000 WOZ, multi-person household, owner-occupied)

---

## Key decisions

| Decision                          | Options Considered                               | Choice Made                        | Reasoning                                                                      |
|-----------------------------------|--------------------------------------------------|------------------------------------|--------------------------------------------------------------------------------|
| Gemeente → waterschap data source | Live API, extend COELO Excel, static JSON        | Static JSON                        | COELO Excel has no waterschap column; JSON is auditable and version-controlled |
| Mapping granularity               | Province-level, municipality-level               | Municipality-level (342 entries)   | Province-level too coarse — municipalities can span waterschap boundaries      |
| Missing waterschap result         | Throw exception, empty result, nullable property | Nullable `WaterschapLastenResult?` | Graceful degradation — a mapping gap should not crash valid gemeente lookups   |

---

## Verified output

Leiden · €511,000 WOZ · multi-person · owner:

| Tax                          | Amount        |
|------------------------------|---------------|
| Afvalstoffenheffing          | €483.96       |
| OZB                          | €578.96       |
| Rioolheffing                 | €219.00       |
| **Gemeentelijk totaal**      | **€1,281.92** |
| Zuiveringsheffing (Rijnland) | €285.72       |
| Watersysteem ingezetenen     | €134.64       |
| Watersysteem gebouwd         | €118.04       |
| Wegenheffing                 | €0.00         |
| **Waterschap totaal**        | **€538.40**   |

---

## Known gaps

| # | Location                    | Description                                                                                                                            |
|---|-----------------------------|----------------------------------------------------------------------------------------------------------------------------------------|
| 1 | `Program.cs`                | Console still references `Lasten.Infrastructure` directly — unavoidable without a DI container at the composition root                 |
| 2 | `GemeenteWaterschapMapping` | Municipalities straddling waterschap borders may be assigned to the wrong authority — not verified against official waterschapsgrenzen |
| 3 | All projects                | No automated tests exist; domain logic correctness verified by manual console output only                                              |

---

## Next up

- [ ] Add `Lasten.Application.Tests` — priority cases: unknown gemeente name, renter (OZB = 0), single-person household, gemeente with no waterschap mapping
- [ ] Add CLI argument parsing: gemeente name, WOZ value, `--single`/`--multi`, `--owner`/`--renter`
- [ ] Remove `Console.ReadKey()` once CLI args are in place
- [ ] Explore WOZ-waardeloket API for automatic WOZ lookup by address
- [ ] Document the annual COELO update process
