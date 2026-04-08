using Lasten.Application.Ports;
using Lasten.Domain.Gemeentelijkebelastingen;
using Lasten.Domain.Waterschapsbelastingen;

namespace Lasten.Application.Belasting;

public sealed class BerekenBelastingUseCase(
    IGemeenteRepository gemeenteRepository,
    IWaterschappenRepository waterschappen,
    IGemeenteWaterschapMapping mapping)
{
    public Result<BerekenBelastingResult, string> Handle(BerekenBelastingQuery query)
    {
        var gemeente = gemeenteRepository.GetByName(query.GemeenteNaam);
        if (gemeente is null)
            return Result<BerekenBelastingResult, string>.Failure($"Gemeente '{query.GemeenteNaam}' not found.");

        var gemeentelijkeBelastigen = new GemeentelijkeBelastigen(
            query.IsSingleHouseHolder,
            query.IsPropertyOwner,
            query.WozWaarde,
            gemeente);

        var gemeentelijkeLasten = new GemeentelijkeLastenResult(
            Name: gemeente.Name,
            gemeentelijkeBelastigen.Afvalstoffenheffing,
            gemeentelijkeBelastigen.Ozb,
            gemeentelijkeBelastigen.Rioolheffing);

        WaterschapLastenResult? waterschapLasten = null;
        var waterschapCode = mapping.GetWaterschapCode(gemeente.Code);
        var waterschap = waterschapCode is not null ? waterschappen.GetByCode(waterschapCode) : null;
        if (waterschap is not null)
        {
            var waterschapBelastingen = new WaterschapBelastingen(
                waterschap,
                query.IsSingleHouseHolder,
                query.IsPropertyOwner,
                query.WozWaarde);

            waterschapLasten = new WaterschapLastenResult(
                Name: waterschap.Name,
                waterschapBelastingen.Zuiveringsheffing,
                waterschapBelastingen.WatersysteemIngezetenen,
                waterschapBelastingen.WatersysteemGebouwd,
                waterschapBelastingen.Wegenheffing);
        }

        return Result<BerekenBelastingResult, string>.Success(new BerekenBelastingResult(gemeentelijkeLasten, waterschapLasten));
    }
}
