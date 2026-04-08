namespace Lasten.Application.Ports;

/// <summary>
/// Maps a municipality code (CBS gemeentecode) to a water authority code as used in the COELO dataset.
/// </summary>
public interface IGemeenteWaterschapMapping
{
    /// <returns>The waterschap code, or <c>null</c> if no mapping exists for the given gemeente.</returns>
    string? GetWaterschapCode(string gemeenteCode);
}
