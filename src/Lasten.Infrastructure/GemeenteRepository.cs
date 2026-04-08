using System.Collections.Frozen;
using Lasten.Application.Ports;
using Lasten.Domain.Gemeentelijkebelastingen;

namespace Lasten.Infrastructure;

public sealed class GemeenteRepository : IGemeenteRepository
{
    private readonly FrozenDictionary<string, Gemeente> _gemeenten = GemeentenLoader.Load();

    public Gemeente? GetByName(string name) => _gemeenten.GetValueOrDefault(name);
}
