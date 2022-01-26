using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Net6Features.Shared.Models;
using Net6Features.Shared.Services;
using ProtoBuf.Grpc.Client;

namespace Net6Features.Client.Pages
{
    public partial class Speakers : IDisposable
    {
        [Inject] public GrpcChannel? GrpcChannel { get; set; }

        private ISpeakersService? _speakersService;

        protected override async Task<List<Speaker>> LoadDataFromGrpcService(int take, string searchTerm)
        {
            if (GrpcChannel != null)
            {
                _speakersService = GrpcChannel.CreateGrpcService<ISpeakersService>();
                return await _speakersService.GetCollectionAsync(new CollectionRequest { Take = take, SearchTerm = searchTerm });
            }
            return new List<Speaker>();
        }
    }
}