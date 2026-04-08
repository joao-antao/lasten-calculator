namespace Lasten.Application.Belastingen;

public sealed record BerekenLastenQuery(
    string GemeenteNaam,
    decimal WozWaarde,
    bool IsSingleHouseHolder,
    bool IsPropertyOwner);
