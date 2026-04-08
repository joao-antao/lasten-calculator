using Lasten.Application.Ports;
using Lasten.Domain;
using Lasten.Domain.Gemeentelijkebelastingen;
using Lasten.Domain.Waterschapsbelastingen;

namespace Lasten.Application.Taxes;

public sealed class CalculateTaxesUseCase(
    IGemeentenRepository gemeenten,
    IWaterschappenRepository waterschappen,
    IGemeenteWaterschapMapping mapping)
{
    public Result<CalculateTaxesResult, string> Handle(CalculateTaxesQuery query)
    {
        var gemeente = gemeenten.GetByName(query.GemeenteNaam);
        if (gemeente is null)
            return Result<CalculateTaxesResult, string>.Failure($"Gemeente '{query.GemeenteNaam}' not found.");

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
        var waterschap = waterschapCode is not null ? waterschappen.GetByCode(waterschapCode) : null;
        if (waterschap is not null)
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

        return Result<CalculateTaxesResult, string>.Success(new CalculateTaxesResult(gemeentelijkeLasten, waterschapLasten));
    }
}
