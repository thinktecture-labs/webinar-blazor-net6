using System.ServiceModel;

namespace Net6Features.Shared.Services
{
    [ServiceContract]
    public interface IDataService<T>
    {
        Task<List<T>> CollectionAsync();
    }
}
