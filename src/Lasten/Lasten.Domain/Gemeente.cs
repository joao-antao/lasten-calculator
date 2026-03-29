namespace Lasten.Domain;

/// <summary>
/// Represents a municipality in the Netherlands with its municipal tax rates.
/// Contains tariffs for property tax (OZB), waste tax (afvalstoffenheffing), and sewerage charge (rioolheffing).
/// </summary>
/// <param name="Code">The municipality code (e.g., "0363" for Amsterdam).</param>
/// <param name="Name">The name of the municipality.</param>
/// <param name="OzbTarief">Property tax rate as a percentage of WOZ value (e.g., 0.0577 = 0.0577%).</param>
/// <param name="Afval1P">Annual waste tax for a single-person household in euros.</param>
/// <param name="AfvalMP">Annual waste tax for a multi-person household in euros.</param>
/// <param name="Riool1P">Annual sewerage charge for a single-person household in euros.</param>
/// <param name="RioolMP">Annual sewerage charge for a multi-person household in euros.</param>
public sealed record Gemeente(
    string Code,
    string Name,
    decimal OzbTarief,
    decimal Afval1P,
    decimal AfvalMP,
    decimal Riool1P,
    decimal RioolMP
);
