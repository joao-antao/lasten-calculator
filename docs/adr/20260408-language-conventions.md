# ADR: Language Conventions — Dutch Domain Terms, English Structure

📅 2026-04-08

## Context

The codebase mixes Dutch and English identifiers. Dutch tax concepts (`Afvalstoffenheffing`, `WozWaarde`, `Zuiveringsheffing`) have no clean English equivalent and are the statutory terms used in Dutch law and the COELO dataset. Using translated names would obscure their meaning and create a mismatch with source data. At the same time, structural and architectural names (`Repository`, `UseCase`, `Query`) are English conventions known in the Software Engineering industry.

Without a documented rule, contributors make inconsistent choices — some translating domain terms, others not.

## Decision

Apply a two-tier language rule:

| Tier                         | Language | What belongs here                                                                                                                                                                         |
|------------------------------|----------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Domain**                   | Dutch    | Tax types, tariff names, levies, statutory entity names that appear verbatim in Dutch                                                                                                     |
| **Architecture & structure** | English  | Layer types (`Repository`, `UseCase`, `Query`, `Handler`), properties with no Dutch statutory meaning (`Name`, `Code`, `IsSingleHouseHolder`, `IsPropertyOwner`), infrastructure plumbing |


### Examples

| Identifier                | Language     | Reason                                                                  |
|---------------------------|--------------|-------------------------------------------------------------------------|
| `Afvalstoffenheffing`     | 🇳🇱 Dutch   | Statutory tax name                                                      |
| `Zuiveringsheffing`       | 🇳🇱 Dutch   | Statutory tax name                                                      |
| `WozWaarde`               | 🇳🇱 Dutch   | Legally defined property valuation concept                              |
| `OzbTarief`               | 🇳🇱 Dutch   | Statutory abbreviation                                                  |
| `Gemeente`                | 🇳🇱 Dutch   | Dutch legal entity with no direct English equivalent                    |
| `Waterschap`              | 🇳🇱 Dutch   | Dutch legal entity with no direct English equivalent                    |
| `BerekenBelastingUseCase` | 🇳🇱 + 🇬🇧  | Dutch domain action (`BerekenBelasting`) + English architectural suffix |
| `BerekenBelastingQuery`   | 🇳🇱 + 🇬🇧  | Dutch domain noun + English structural suffix                           |
| `BerekenBelastingResult`  | 🇳🇱 + 🇬🇧  | Dutch domain noun + English structural suffix                           |
| `GemeenteRepository`      | 🇳🇱 + 🇬🇧  | Dutch entity name + English architectural suffix                        |
| `IsSingleHouseHolder`     | 🇬🇧 English | Boolean flag — no Dutch statutory meaning                               |

### Boundaries

- **Namespaces** follow the project structure (`Lasten.Domain.Gemeentelijkebelastingen`) — Dutch nouns are acceptable here because they scope domain concepts
- **Do not translate** Dutch statutory terms into English to appear consistent — the loss of traceability to source data outweighs the cosmetic gain
- **Do not use Dutch** for generic structural roles: suffixes like `Repository`, `Handler`, `UseCase`, `Query`, `Result` stay English

## Consequences

✅ Domain identifiers are traceable to the COELO dataset and Dutch tax law  
✅ Structural code follows familiar .NET conventions  
⚠️ New contributors unfamiliar with Dutch tax terminology must consult the domain layer documentation  

## Next steps

- Apply this rule consistently when reviewing PRs — flag Dutch identifiers used for structural roles and vice versa
- Ensure XML doc comments on domain types explain the Dutch terms in English (already in place for `Gemeente`, `Waterschap`, `GemeentelijkeBelastigen`)
