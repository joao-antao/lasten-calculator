using Lasten.Application.Ports;
using Lasten.Domain.Gemeentelijkebelastingen;
using Lasten.Domain.Waterschapsbelastingen;

namespace Lasten.Application.Belastingen;

public sealed class BerekenLastenHandler(
    IGemeentenRepository gemeenten,
    IWaterschappenRepository waterschappen,
    IGemeenteWaterschapMapping mapping)
{
    public BerekenLastenResult Handle(BerekenLastenQuery query)
    {
        var gemeente = gemeenten.GetAll().FirstOrDefault(g => g.Name == query.GemeenteNaam)
            ?? throw new ArgumentException($"Gemeente '{query.GemeenteNaam}' not found.", nameof(query));

        var gemeentelijkeBelastigen = new GemeentelijkeBelastigen(
            query.IsSingleHouseHolder,
            query.IsPropertyOwner,
            query.WozWaarde,
            gemeente);

        var gemeentelijkeLasten = new GemeentelijkeLastenResult(
            gemeente.Name,
            gemeentelijkeBelastigen.Afvalstoffenheffing,
            gemeentelijkeBelastigen.Ozb,
            gemeentelijkeBelastigen.Rioolheffing);

        WaterschapLastenResult? waterschapLasten = null;
        var waterschapCode = mapping.GetWaterschapCode(gemeente.Code);
        if (waterschapCode is not null && waterschappen.GetAll().TryGetValue(waterschapCode, out var waterschap))
        {
            var waterschapBelastingen = new WaterschapBelastingen(
                waterschap,
                query.IsSingleHouseHolder,
                query.IsPropertyOwner,
                query.WozWaarde);

            waterschapLasten = new WaterschapLastenResult(
                waterschap.Name,
                waterschapBelastingen.Zuiveringsheffing,
                waterschapBelastingen.WatersysteemIngezetenen,
                waterschapBelastingen.WatersysteemGebouwd,
                waterschapBelastingen.Wegenheffing);
        }

        return new BerekenLastenResult(gemeentelijkeLasten, waterschapLasten);
    }
}
