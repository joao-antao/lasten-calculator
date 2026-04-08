namespace Lasten.Application.Belasting;

public sealed record GemeentelijkeLastenResult(
    string Name,
    decimal Afvalstoffenheffing,
    decimal Ozb,
    decimal Rioolheffing)
{
    public decimal Total => Afvalstoffenheffing + Ozb + Rioolheffing;
}

public sealed record WaterschapLastenResult(
    string Name,
    decimal Zuiveringsheffing,
    decimal WatersysteemIngezetenen,
    decimal WatersysteemGebouwd,
    decimal Wegenheffing)
{
    public decimal Total => Zuiveringsheffing + WatersysteemIngezetenen + WatersysteemGebouwd + Wegenheffing;
}

public sealed record BerekenBelastingResult(
    GemeentelijkeLastenResult GemeentelijkeLasten,
    WaterschapLastenResult? WaterschapLasten);
