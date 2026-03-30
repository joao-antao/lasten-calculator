using Lasten.Domain;
using Lasten.Infrastructure;

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