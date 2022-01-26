using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Net6Features.Shared.Models;
using Net6Features.Shared.Services;
using ProtoBuf.Grpc.Client;

namespace Net6Features.Client.Pages
{
    public partial class Conferences
    {
        [Inject] public GrpcChannel? GrpcChannel { get; set; }

        private IConferenceService? _conferencesService;

        protected override async Task<List<Conference>> LoadDataFromGrpcService(int take, string searchTerm)
        {
            if (GrpcChannel != null)
            {
                _conferencesService = GrpcChannel.CreateGrpcService<IConferenceService>();
                return await _conferencesService.GetCollectionAsync(new CollectionRequest { Take = take, SearchTerm = searchTerm });
            }
            return new List<Conference>();
        }
    }
}