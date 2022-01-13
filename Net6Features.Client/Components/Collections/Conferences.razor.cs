using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Net6Features.Shared.Models;
using Net6Features.Shared.Services;
using ProtoBuf.Grpc.Client;

namespace Net6Features.Client.Components.Collections
{
    public partial class Conferences
    {
        [Inject] public GrpcChannel? GrpcChannel { get; set; }

        private IConferenceService? _conferencesService;
        protected override async Task<List<Conference>> LoadDataWithGrpc()
        {
            if (GrpcChannel != null)
            {
                _conferencesService = GrpcChannel.CreateGrpcService<IConferenceService>();
                return await _conferencesService.CollectionAsync();
            }
            return new List<Conference>();
        }
    }
}