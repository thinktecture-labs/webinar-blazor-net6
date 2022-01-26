using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Net6Features.Shared.Models;
using Net6Features.Shared.Services;
using ProtoBuf.Grpc.Client;

namespace Net6Features.Client.Pages
{
    public partial class Contributions : IDisposable
    {
        [Inject] public GrpcChannel? GrpcChannel { get; set; }

        private IContributionsService? _contributionService;
        protected override async Task<List<Contribution>> LoadDataFromGrpcService(int take, string searchTerm)
        {
            if (GrpcChannel != null)
            {
                _contributionService = GrpcChannel.CreateGrpcService<IContributionsService>();
                return await _contributionService.GetCollectionAsync(new CollectionRequest { Take = take, SearchTerm = searchTerm });
            }
            return new List<Contribution>();
        }
    }
}