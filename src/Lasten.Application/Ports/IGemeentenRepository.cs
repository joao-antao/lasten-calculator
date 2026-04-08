using Lasten.Domain.Gemeentelijkebelastingen;

namespace Lasten.Application.Ports;

public interface IGemeentenRepository
{
    Gemeente? GetByName(string name);
}
