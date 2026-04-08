namespace Lasten.Application.Taxes;

public sealed record CalculateTaxesQuery(
    string GemeenteNaam,
    decimal WozWaarde,
    bool IsSingleHouseHolder,
    bool IsPropertyOwner);
