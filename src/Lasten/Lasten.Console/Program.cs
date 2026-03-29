using Lasten.Domain;
using Lasten.Infrastructure;

// This is the COELO dataset for 2025
var file = Path.Combine(AppContext.BaseDirectory, "Gemeentelijke_belastingen_2025.xlsx");
var gemeenten = GemeentenLoader.Load(file);

var gemeentelijkeBelastigen = new GemeentelijkeBelastigen(
    IsSingleHouseHolder: false,
    IsPropertyOwner: true,
    WozWaarde: 511000m,
    Gemeente: gemeenten.First(g => g.Name == "Amsterdam")
);

Console.WriteLine(gemeentelijkeBelastigen.Afvalstoffenheffing);
Console.WriteLine(gemeentelijkeBelastigen.Ozb);
Console.WriteLine(gemeentelijkeBelastigen.Rioolheffing);
