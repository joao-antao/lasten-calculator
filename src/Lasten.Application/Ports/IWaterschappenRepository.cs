using Lasten.Domain.Waterschapsbelastingen;

namespace Lasten.Application.Ports;

public interface IWaterschappenRepository
{
    Waterschap? GetByCode(string code);
}
