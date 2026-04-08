using Lasten.Application.Ports;
using Lasten.Domain.Waterschapsbelastingen;

namespace Lasten.Infrastructure;

public sealed class WaterschappenRepository : IWaterschappenRepository
{
    private readonly IReadOnlyDictionary<string, Waterschap> _waterschappen = WaterschapLoader.Load();

    public IReadOnlyDictionary<string, Waterschap> GetAll() => _waterschappen;
}
