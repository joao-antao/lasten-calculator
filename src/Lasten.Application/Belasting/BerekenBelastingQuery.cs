namespace Lasten.Application.Belasting;

public sealed record BerekenBelastingQuery(
    string GemeenteNaam,
    decimal WozWaarde,
    bool IsSingleHouseHolder,
    bool IsPropertyOwner);
