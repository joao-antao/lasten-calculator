using Lasten.Domain.Gemeentelijkebelastingen;

namespace Lasten.Application.Ports;

public interface IGemeentenRepository
{
    IReadOnlyList<Gemeente> GetAll();
}
