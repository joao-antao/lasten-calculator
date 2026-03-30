namespace Lasten.Domain.Waterschapsbelastingen;

/// <summary>
/// Calculates the water authority (waterschap) taxes owed by a specific household.
/// Takes the tariff data from a <see cref="Waterschap"/> and applies it to the
/// household's situation( number of occupants, ownership status, and WOZ value)
/// to produce the levies and the <see cref="Total"/> annual amount.
/// </summary>
/// <param name="Waterschap">The water authority whose tariffs are used for the calculation.</param>
/// <param name="IsSingleHouseHolder">
/// <c>true</c> if the household has a single occupant; determines whether
/// <see cref="Waterschap.ZuiveringsheffingEen"/> or <see cref="Waterschap.ZuiveringsheffingMeer"/>
/// is applied for the sewage treatment levy.
/// </param>
/// <param name="IsPropertyOwner">
/// <c>true</c> if the household owns the property. Only owners pay
/// <see cref="WatersysteemGebouwd"/>; renters receive <c>0</c> for that levy.
/// </param>
/// <param name="WozWaarde">
/// The official WOZ (property valuation) of the home in euros. Used to calculate
/// <see cref="WatersysteemGebouwd"/> as a percentage of this value.
/// Ignored when <paramref name="IsPropertyOwner"/> is <c>false</c>.
/// </param>
public sealed record WaterschapBelastingen(
    Waterschap Waterschap,
    bool IsSingleHouseHolder,
    bool  IsPropertyOwner,
    decimal WozWaarde)
{
    public string Name => Waterschap.Name;
    
    public decimal Zuiveringsheffing => IsSingleHouseHolder ? Waterschap.ZuiveringsheffingEen : Waterschap.ZuiveringsheffingMeer;

    public decimal WatersysteemIngezetenen => Waterschap.WatersysteemIngezetenen;
    
    public decimal WatersysteemGebouwd => IsPropertyOwner ? WozWaarde * (Waterschap.WatersysteemGebouwd / 100m) : 0m;
    
    public decimal Wegenheffing => Waterschap.WegenIngezetenen;
    
    public decimal Total => Zuiveringsheffing + WatersysteemIngezetenen + WatersysteemGebouwd + Wegenheffing;
}
