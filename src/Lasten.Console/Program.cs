using Lasten.Domain;
using Lasten.Infrastructure;

// Loads the COELO dataset from 2025
var gemeenten = GemeentenLoader.Load();

var gemeentelijkeBelastigen = new GemeentelijkeBelastigen(
    IsSingleHouseHolder: false,
    IsPropertyOwner: true,
    WozWaarde: 511000m,
    Gemeente: gemeenten.First(g => g.Name == "Amsterdam")
);

Console.WriteLine(gemeentelijkeBelastigen.Afvalstoffenheffing);
Console.WriteLine(gemeentelijkeBelastigen.Ozb);
Console.WriteLine(gemeentelijkeBelastigen.Rioolheffing);