# Sprint 0 — Initial working calculator

📅 2026-04-08

## Goal

Deliver a working console tool that calculates Dutch municipal taxes (*gemeentelijke belastingen*) and water authority taxes (*waterschapsbelastingen*) for a given household and property.

---

## What was built

### Domain — `Lasten.Domain`

**Gemeentelijke belastingen**

| Type | Description |
|---|---|
| `Gemeente` | Municipality with OZB tariff, waste tax rates, and sewerage charge rates |
| `GemeentelijkeBelastigen` | Calculates Afvalstoffenheffing, OZB, and Rioolheffing given household inputs |

Calculation rules:
- **Afvalstoffenheffing** — flat fee from gemeente rates; `Afval1P` for single-person, `AfvalMP` for multi-person
- **OZB** — `WozWaarde × (OzbTarief / 100)`; only paid by property owners, zero for renters
- **Rioolheffing** — flat fee from gemeente rates; `Riool1P` for single-person, `RioolMP` for multi-person
- All values rounded to 2 decimal places with `MidpointRounding.AwayFromZero`

**Waterschapsbelastingen**

| Type | Description |
|---|---|
| `Waterschap` | Water authority with zuivering, watersysteem, and wegen tariffs |
| `WaterschapBelastingen` | Calculates Zuiveringsheffing, WatersysteemIngezetenen, WatersysteemGebouwd, Wegenheffing |

Calculation rules:
- **Zuiveringsheffing** — flat fee; `ZuiveringsheffingEen` for single-person, `ZuiveringsheffingMeer` for multi-person
- **WatersysteemIngezetenen** — flat fee for all residents
- **WatersysteemGebouwd** — `WozWaarde × (WatersysteemGebouwd / 100)`; owners only
- **Wegenheffing** — flat fee; `0` for waterschappen that don't levy this charge

### Infrastructure — `Lasten.Infrastructure`

| Component | Description |
|---|---|
| `GemeentenLoader` | Reads `Gemeentelijke_belastingen_2025.xlsx` (COELO) from `Coelo/` at startup |
| `WaterschapLoader` | Reads `Waterschapsbelastingen_2025.xlsx` (COELO) from `Coelo/` at startup |
| `XLCellExtensions` | `GetDecimalOrDefault()` helper — returns `0m` for missing/empty cells |

### Console — `Lasten.Console`

Hardcoded example for Leiden (gemeente code lookup by name, waterschap code `0616` = Rijnland):
- WOZ value: €511,000
- Multi-person household
- Property owner

Outputs each tax line and a subtotal for both gemeentelijke and waterschapsbelastingen.

---

## Known gaps (TODOs in code)

| # | Location     | Description                                                                                                   |
|---|--------------|---------------------------------------------------------------------------------------------------------------|
| 1 | `Program.cs` | Console references Infrastructure directly — should go through an Application layer                           |
| 2 | `Program.cs` | WOZ value is hardcoded — should be fetched from [WOZ-waardeloket](https://www.wozwaardeloket.nl) (public API) |
| 3 | `Program.cs` | Municipality → water authority mapping is missing — currently requires manual code lookup                     |

---

## Next up

- [ ] Introduce `Lasten.Application` with command/query handlers to decouple console from infrastructure
- [ ] Build a municipality → water authority mapping (342 gemeenten → 21 waterschappen)