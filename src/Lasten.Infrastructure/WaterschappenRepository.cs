using System.Collections.Frozen;
using Lasten.Application.Ports;
using Lasten.Domain.Waterschapsbelastingen;

namespace Lasten.Infrastructure;

public sealed class WaterschappenRepository : IWaterschappenRepository
{
    private readonly FrozenDictionary<string, Waterschap> _waterschappen = WaterschapLoader.Load();

    public Waterschap? GetByCode(string code) => _waterschappen.GetValueOrDefault(code);
}
