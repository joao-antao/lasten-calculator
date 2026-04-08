using Lasten.Application.Belastingen;
using Lasten.Infrastructure;

// Composition root: wire up infrastructure adapters
var handler = new BerekenLastenHandler(
    gemeenten: new GemeentenRepository(),
    waterschappen: new WaterschappenRepository(),
    mapping: new GemeenteWaterschapMapping());

var result = handler.Handle(new BerekenLastenQuery(
    GemeenteNaam: "Leiden",
    WozWaarde: 511000m,
    IsSingleHouseHolder: false,
    IsPropertyOwner: true));

Console.WriteLine("Gemeentelijke belastingen — " + result.GemeentelijkeLasten.GemeenteNaam);
Console.WriteLine("  Afvalstoffenheffing : " + result.GemeentelijkeLasten.Afvalstoffenheffing);
Console.WriteLine("  OZB                 : " + result.GemeentelijkeLasten.Ozb);
Console.WriteLine("  Rioolheffing        : " + result.GemeentelijkeLasten.Rioolheffing);
Console.WriteLine("  Totaal              : " + result.GemeentelijkeLasten.Totaal);

if (result.WaterschapLasten is { } ws)
{
    Console.WriteLine();
    Console.WriteLine("Waterschapsbelastingen — " + ws.WaterschapNaam);
    Console.WriteLine("  Zuiveringsheffing       : " + ws.Zuiveringsheffing);
    Console.WriteLine("  Watersysteem ingezetenen: " + ws.WatersysteemIngezetenen);
    Console.WriteLine("  Watersysteem gebouwd    : " + ws.WatersysteemGebouwd);
    Console.WriteLine("  Wegenheffing            : " + ws.Wegenheffing);
    Console.WriteLine("  Totaal                  : " + ws.Totaal);
}
else
{
    Console.WriteLine();
    Console.WriteLine("Waterschapsbelastingen: geen koppeling gevonden voor deze gemeente.");
}

Console.ReadKey();