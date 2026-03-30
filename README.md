# lasten-calculator

A console tool that calculates Dutch municipality and water taxes for a given household and property, based on official dataset from the [COELO](https://www.coelo.nl/) annual workbooks.

> **Disclaimer:** No guarantee is made for the accuracy of the results. Please consult a qualified professional for tax advice.

## Supported taxes

* Gemeentelijke belastingen (municipal taxes)
* Waterschapsbelastingen (water authority taxes)

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- The COELO Excel workbook (`Gemeentelijke_belastingen_2025.xlsx`) placed under `src/Lasten.Infrastructure/Coelo/`

## Data source

Tax rates are sourced from the **COELO** (*Centrum voor Onderzoek van de Economie van de Lagere Overheden*) annual publication on Dutch municipal taxes.
