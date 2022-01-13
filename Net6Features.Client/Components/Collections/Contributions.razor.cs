using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Net6Features.Shared.Models;
using Net6Features.Shared.Services;
using ProtoBuf.Grpc.Client;

namespace Net6Features.Client.Components.Collections
{
    public partial class Contributions
    {
        [Inject] public GrpcChannel? GrpcChannel { get; set; }

        private IContributionsService? _contributionService;
        protected override async Task<List<Contribution>> LoadDataWithGrpc()
        {
            if (GrpcChannel != null)
            {
                _contributionService = GrpcChannel.CreateGrpcService<IContributionsService>();
                return await _contributionService.CollectionAsync();
            }
            return new List<Contribution>();            
        }
    }
}