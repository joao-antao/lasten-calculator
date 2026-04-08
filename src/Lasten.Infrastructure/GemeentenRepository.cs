using Lasten.Application.Ports;
using Lasten.Domain.Gemeentelijkebelastingen;

namespace Lasten.Infrastructure;

public sealed class GemeentenRepository : IGemeentenRepository
{
    private readonly IReadOnlyList<Gemeente> _gemeenten = GemeentenLoader.Load();

    public IReadOnlyList<Gemeente> GetAll() => _gemeenten;
}
