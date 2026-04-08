namespace Lasten.Application.Taxes;

public sealed record GemeentelijkeLastenResult(
    string GemeenteNaam,
    decimal Afvalstoffenheffing,
    decimal Ozb,
    decimal Rioolheffing)
{
    public decimal Total => Afvalstoffenheffing + Ozb + Rioolheffing;
}

public sealed record WaterschapLastenResult(
    string WaterschapNaam,
    decimal Zuiveringsheffing,
    decimal WatersysteemIngezetenen,
    decimal WatersysteemGebouwd,
    decimal Wegenheffing)
{
    public decimal Total => Zuiveringsheffing + WatersysteemIngezetenen + WatersysteemGebouwd + Wegenheffing;
}

public sealed record CalculateTaxesResult(
    GemeentelijkeLastenResult GemeentelijkeLasten,
    WaterschapLastenResult? WaterschapLasten);
