using Lasten.Application.Taxes;
using Lasten.Infrastructure;

var useCase = new CalculateTaxesUseCase(
    gemeenten: new GemeentenRepository(),
    waterschappen: new WaterschappenRepository(),
    mapping: new GemeenteWaterschapMapping());

var result = useCase.Handle(new CalculateTaxesQuery(
    GemeenteNaam: "Leiden",
    WozWaarde: 511000m,
    IsSingleHouseHolder: false,
    IsPropertyOwner: true));

if (result.IsFailure)
{
    Console.Error.WriteLine("Upsy " + result.Error);
    Console.ReadKey();
    return;
}

var berekening = result.Value;

Console.WriteLine("Gemeentelijke belastingen — " + berekening.GemeentelijkeLasten.GemeenteNaam);
Console.WriteLine("  Afvalstoffenheffing : " + berekening.GemeentelijkeLasten.Afvalstoffenheffing);
Console.WriteLine("  OZB                 : " + berekening.GemeentelijkeLasten.Ozb);
Console.WriteLine("  Rioolheffing        : " + berekening.GemeentelijkeLasten.Rioolheffing);
Console.WriteLine("  Totaal              : " + berekening.GemeentelijkeLasten.Total);

if (berekening.WaterschapLasten is { } ws)
{
    Console.WriteLine();
    Console.WriteLine("Waterschapsbelastingen — " + ws.WaterschapNaam);
    Console.WriteLine("  Zuiveringsheffing       : " + ws.Zuiveringsheffing);
    Console.WriteLine("  Watersysteem ingezetenen: " + ws.WatersysteemIngezetenen);
    Console.WriteLine("  Watersysteem gebouwd    : " + ws.WatersysteemGebouwd);
    Console.WriteLine("  Wegenheffing            : " + ws.Wegenheffing);
    Console.WriteLine("  Totaal                  : " + ws.Total);
}
else
{
    Console.WriteLine();
    Console.WriteLine("Waterschapsbelastingen: we could not find the water authority for this municipality: (" + berekening.GemeentelijkeLasten.GemeenteNaam + ")");
}

Console.ReadKey();