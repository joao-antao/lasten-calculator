using Lasten.Domain.Gemeentelijkebelastingen;
using Lasten.Domain.Waterschapsbelastingen;
using Lasten.Infrastructure;

var gemeenten = GemeentenLoader.Load();
var waterschappen = WaterschapLoader.Load();

// Todo: Fetch WOZ from WOZ-waardeloket	wozwaardeloket.nl (public)

var gemeentelijkeBelastigen = new GemeentelijkeBelastigen(
    IsSingleHouseHolder: false,
    IsPropertyOwner: true,
    WozWaarde: 511000m,
    Gemeente: gemeenten.First(g => g.Name == "Leiden")
);

Console.WriteLine("Gemeentelijke belastingen");
Console.WriteLine("Afvalstoffenheffing: " + gemeentelijkeBelastigen.Afvalstoffenheffing);
Console.WriteLine("Ozb: " + gemeentelijkeBelastigen.Ozb);
Console.WriteLine("Rioolheffing: " + gemeentelijkeBelastigen.Rioolheffing);

// Todo: A mapping between the 21 water authorities and the municipality
var waterschapBelastingen = new WaterschapBelastingen( 
    waterschappen.First(w => w.Key == "0616").Value, // Leiden -> Rijnland (e.g: 0546 -> 0616)
    IsSingleHouseHolder: false, 
    IsPropertyOwner: true, 
    WozWaarde: 511000m);

Console.WriteLine("Waterschapsbelastingen " + waterschapBelastingen.Name);
Console.WriteLine("Zuiveringsheffing: "+ waterschapBelastingen.Zuiveringsheffing);
Console.WriteLine("Wegenheffing "+ waterschapBelastingen.Wegenheffing);
Console.WriteLine("WatersysteemGebouwd: "+ waterschapBelastingen.WatersysteemGebouwd);
Console.WriteLine("WatersysteemIngezetenen: "+ waterschapBelastingen.WatersysteemIngezetenen);
Console.WriteLine("Total: "+ waterschapBelastingen.Total);

Console.ReadKey();