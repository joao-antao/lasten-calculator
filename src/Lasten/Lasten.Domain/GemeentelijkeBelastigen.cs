namespace Lasten.Domain;

/// <summary>
/// Calculates municipal taxes (gemeentelijke belastingen) based on household composition, property ownership status, and WOZ valuation.
/// Includes waste tax (afvalstoffenheffing), property tax (OZB), and sewerage charge (rioolheffing).
/// </summary>
/// <param name="IsSingleHouseHolder">Whether the household is a single person.</param>
/// <param name="IsPropertyOwner">Whether the resident is a property owner.</param>
/// <param name="WozWaarde">The municipal valuation of the property in euros.</param>
/// <param name="Gemeente">The municipality rates and tariffs.</param>
public sealed record GemeentelijkeBelastigen(bool IsSingleHouseHolder, bool IsPropertyOwner, decimal WozWaarde, Gemeente Gemeente)
{
    /// <summary>
    /// Waste tax (afvalstoffenheffing) based on household size (single or multi-person).
    /// Who pays: Property owners only, not renters.
    /// </summary>
    public decimal Afvalstoffenheffing => IsSingleHouseHolder ? Gemeente.Afval1P : Gemeente.AfvalMP;

    /// <summary>
    /// Property tax (onroerendezaakbelasting) for property owners, calculated as a percentage of the WOZ value.
    /// Who pays: Residents (both owners and renters).
    /// </summary>
    public decimal Ozb => IsPropertyOwner ? WozWaarde * (Gemeente.OzbTarief / 100m) : 0m;

    /// <summary>
    /// Sewerage charge (rioolheffing) based on household size (single or multi-person).
    /// Who pays: Property owners (sometimes passed to renters via service charges).
    /// </summary>
    public decimal Rioolheffing => IsSingleHouseHolder ? Gemeente.Riool1P : Gemeente.RioolMP;
}