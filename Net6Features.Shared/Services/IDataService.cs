using System.Runtime.Serialization;
using System.ServiceModel;

namespace Net6Features.Shared.Services
{
    [ServiceContract]
    public interface IDataService<T>
    {
        [OperationContract]
        Task<List<T>> GetCollectionAsync(CollectionRequest request);
    }

    [DataContract]
    public class CollectionRequest
    {
        [DataMember(Order = 1)]
        public int Take { get; set; }

        [DataMember(Order = 2)]
        public string SearchTerm { get; set; }
    }
}
