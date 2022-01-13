using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Net6Features.Shared.Models;
using Net6Features.Shared.Services;
using ProtoBuf.Grpc.Client;

namespace Net6Features.Client.Components.Collections
{
    public partial class Speakers
    {
        [Inject] public GrpcChannel? GrpcChannel { get; set; }

        private ISpeakersService? _speakersService;
        protected override async Task<List<Speaker>> LoadDataWithGrpc()
        {
            if (GrpcChannel != null)
            {
                _speakersService = GrpcChannel.CreateGrpcService<ISpeakersService>();
                return await _speakersService.CollectionAsync();
            }
            return new List<Speaker>();
        }
    }
}