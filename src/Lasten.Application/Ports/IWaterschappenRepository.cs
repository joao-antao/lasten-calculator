using Lasten.Domain.Waterschapsbelastingen;

namespace Lasten.Application.Ports;

public interface IWaterschappenRepository
{
    IReadOnlyDictionary<string, Waterschap> GetAll();
}
