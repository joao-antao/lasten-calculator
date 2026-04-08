# ADR: COELO Excel Workbook as Tax Data Source

📅 2026-04-08

## Context

Dutch municipal and water authority tax rates change annually. We need a reliable, authoritative source for these rates. Options considered:

- **COELO Excel** — published annually by the Centre for Research on the Economics of Lower Governments (*Centrum voor Onderzoek van de Economie van de Lagere Overheden*). Covers all ~342 municipalities and all 21 water authorities.
- **Manual entry** — error-prone, labour-intensive, not maintainable.
- **Live government APIs** — no standardised public API exists for these rates as of 2026.

## Decision

Use the COELO Excel workbook (`Gemeentelijke_belastingen_YYYY.xlsx` / `Waterschapsbelastingen_YYYY.xlsx`) as the primary data source, loaded at startup via `ClosedXML`.

### Loader behaviour

| Loader | Worksheet | First data row | Key columns read |
|---|---|---|---|
| `GemeentenLoader` | `Gegevens per gemeente` | 5 | Code, Name, OZB tariff, Afval (1P/MP), Riool (1P/MP) |
| `WaterschapLoader` | `Gegevens per waterschap` | 6 | Code, Name, Zuivering (1P/MP), Watersysteem (ingezetenen/gebouwd), Wegen ingezetenen |

- Rows with blank name or zero OZB + zero waste tax are skipped (aggregate/header rows)
- Municipality codes are left-padded to 4 digits
- All monetary reads use `GetDecimalOrDefault()` — missing cells default to `0`

## Consequences

✅ Single, authoritative annual publication covering all municipalities  
✅ No network dependency at runtime  
⚠️ File must be manually updated each year when COELO publishes new data  
⚠️ Column layout is fragile — if COELO changes the spreadsheet layout, loaders must be updated  
⚠️ No link between municipality codes and their water authority — must be looked up manually today

## Next steps

- Build a municipality → water authority lookup table (21 waterschappen cover all 342 gemeenten)
