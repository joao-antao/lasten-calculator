namespace Lasten.Application.Belastingen;

public sealed record GemeentelijkeLastenResult(
    string GemeenteNaam,
    decimal Afvalstoffenheffing,
    decimal Ozb,
    decimal Rioolheffing)
{
    public decimal Totaal => Afvalstoffenheffing + Ozb + Rioolheffing;
}

public sealed record WaterschapLastenResult(
    string WaterschapNaam,
    decimal Zuiveringsheffing,
    decimal WatersysteemIngezetenen,
    decimal WatersysteemGebouwd,
    decimal Wegenheffing)
{
    public decimal Totaal => Zuiveringsheffing + WatersysteemIngezetenen + WatersysteemGebouwd + Wegenheffing;
}

/// <param name="WaterschapLasten">
/// <c>null</c> when no gemeente → waterschap mapping exists for the requested municipality.
/// </param>
public sealed record BerekenLastenResult(
    GemeentelijkeLastenResult GemeentelijkeLasten,
    WaterschapLastenResult? WaterschapLasten);
