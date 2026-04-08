using Lasten.Application.Belasting;
using Lasten.Infrastructure;

var useCase = new BerekenBelastingUseCase(
    gemeenteRepository: new GemeenteRepository(),
    waterschappen: new WaterschappenRepository(),
    mapping: new GemeenteWaterschapMapping());

var result = useCase.Handle(new BerekenBelastingQuery(
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

Console.WriteLine("Gemeentelijke belastingen — " + berekening.GemeentelijkeLasten.Name);
Console.WriteLine("  Afvalstoffenheffing : " + berekening.GemeentelijkeLasten.Afvalstoffenheffing);
Console.WriteLine("  OZB                 : " + berekening.GemeentelijkeLasten.Ozb);
Console.WriteLine("  Rioolheffing        : " + berekening.GemeentelijkeLasten.Rioolheffing);
Console.WriteLine("  Total               : " + berekening.GemeentelijkeLasten.Total);

if (berekening.WaterschapLasten is { } ws)
{
    Console.WriteLine();
    Console.WriteLine("Waterschapsbelastingen — " + ws.Name);
    Console.WriteLine("  Zuiveringsheffing       : " + ws.Zuiveringsheffing);
    Console.WriteLine("  Watersysteem ingezetenen: " + ws.WatersysteemIngezetenen);
    Console.WriteLine("  Watersysteem gebouwd    : " + ws.WatersysteemGebouwd);
    Console.WriteLine("  Wegenheffing            : " + ws.Wegenheffing);
    Console.WriteLine("  Total                   : " + ws.Total);
}
else
{
    Console.WriteLine();
    Console.WriteLine("Waterschapsbelastingen: we could not find the water authority for this municipality: (" + berekening.GemeentelijkeLasten.Name + ")");
}

Console.ReadKey();