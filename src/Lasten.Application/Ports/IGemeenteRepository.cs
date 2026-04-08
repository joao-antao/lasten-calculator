using Lasten.Domain.Gemeentelijkebelastingen;

namespace Lasten.Application.Ports;

public interface IGemeenteRepository
{
    Gemeente? GetByName(string name);
}
