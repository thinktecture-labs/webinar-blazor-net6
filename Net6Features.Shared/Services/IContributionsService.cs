using Net6Features.Shared.Models;
using System.ServiceModel;

namespace Net6Features.Shared.Services
{
    [ServiceContract]
    public interface IContributionsService : IDataService<Contribution>
    {
    }
}
