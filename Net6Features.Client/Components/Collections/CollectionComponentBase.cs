using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Net6Features.Client.Components.Collections
{
    public abstract class CollectionComponentBase<T> : ComponentBase
        where T : class
    {
        [Inject] public HttpClient? HttpClient { get; set; }
        [Parameter] public string SearchTerm { get; set; } = string.Empty;

        protected List<T> _data = new List<T>();
        protected bool _loadingData = false;
        protected bool _useGrpc = false;
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
            await base.OnInitializedAsync();
        }
        protected abstract Task<List<T>> LoadDataWithGrpc();

        public async Task ReloadDataAsync(bool useGrpc)
        {
            _useGrpc = useGrpc;
            await LoadData();
        }

        private async Task LoadData()
        {
            _loadingData = true;
            try
            {
                await InvokeAsync(async () =>
                {
                    _data = _useGrpc ? await LoadDataWithGrpc() : await LoadDataFromApi();
                    _loadingData = false;
                    StateHasChanged();
                });
            }
            catch (Exception ex)
            {
                var currentType = typeof(T);
                Console.WriteLine($"Load data for {currentType.Name} failed. Error: {ex.Message}");
                throw new ArgumentException($"Es wurden keine Daten für {currentType.Name} gefunden.");
            }
        }

        private async Task<List<T>> LoadDataFromApi()
        {
            if (HttpClient != null)
            {
                var currentType = typeof(T);
                var data = await HttpClient.GetFromJsonAsync<List<T>>($"{currentType.Name}");
                return data ?? new List<T>();
            }
            return new List<T>();
        }
    }
}
