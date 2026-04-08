namespace Lasten.Domain.Waterschapsbelastingen;

/// <summary>
/// Much of the Netherlands is below sea level, the water taxes are mainly used to manage the dikes and dunes (keeping your feet dry) and cleaning wastewater.
/// This value is paid to the regional water authorities. There are 21 in the Netherlands and each one of them can apply different values. See: https://waterschappen.nl/
/// </summary>
/// <param name="Code">The unique code of the regional water authority</param>
/// <param name="Name">The name of the regional water authority</param>
/// <param name="ZuiveringsheffingEen">Purification charge for single-person households </param>
/// <param name="ZuiveringsheffingMeer">Purification charge for multiple person households</param>
/// <param name="WatersysteemIngezetenen">Water system charge for residents</param>
/// <param name="WatersysteemGebouwd">Water system charge for building (specifically for property owners)</param>
/// <param name="WegenIngezetenen">Road charge for residents (not applicable by all)</param>
public sealed record Waterschap(
    string Code,
    string Name,
    decimal ZuiveringsheffingEen,
    decimal ZuiveringsheffingMeer,
    decimal WatersysteemIngezetenen,
    decimal WatersysteemGebouwd,      // % of WOZ value (owners only)
    decimal WegenIngezetenen
);