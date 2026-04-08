using System.Collections.Frozen;
using System.Text.Json;
using Lasten.Application.Ports;

namespace Lasten.Infrastructure;

/// <summary>
/// Maps municipality codes to water authority codes (COELO) using a bundled JSON lookup table.
/// The mapping is based on geographic water authority boundaries for 2025.
/// Source: <c>Coelo/gemeente_waterschap_2025.json</c>
/// </summary>
/// <remarks>
/// Municipalities that cross waterschap boundaries are assigned to the waterschap that covers
/// the majority of the municipality's built-up area. Boundary cases should be verified against
/// the official waterschapsgrenzen published by the Unie van Waterschappen.
/// </remarks>
public sealed class GemeenteWaterschapMapping : IGemeenteWaterschapMapping
{
    private readonly FrozenDictionary<string, string> _mapping = Load();

    public string? GetWaterschapCode(string gemeenteCode) => _mapping.GetValueOrDefault(gemeenteCode);

    private static FrozenDictionary<string, string> Load()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Coelo/gemeente_waterschap_2025.json");
        var json = File.ReadAllText(path);
        var result = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? throw new InvalidOperationException("Failed to load mapping.");
        return result.ToFrozenDictionary();
    }
}
