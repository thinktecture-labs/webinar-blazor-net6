using Microsoft.AspNetCore.Components;
using Net6Features.Client.Components;
using Net6Features.Client.Utils;
using System.Diagnostics;
using System.Net.Http.Json;

namespace Net6Features.Client.Pages
{
    public abstract class CollectionComponentBase<T> : ComponentBase , IDisposable
        where T : class
    {
        [Inject] public PersistentComponentState? ComponentState { get; set; }
        [Inject] public IConfiguration Configuration { get; set; } = default!;

        [Parameter, SupplyParameterFromQuery(Name = "searchTerm")]
        public string SearchTerm { get; set; } = string.Empty;

        private PersistingComponentStateSubscription? _stateSubscription;
        private HttpClient _httpClient = default!;
        private BenchmarkHttpHandler _httpHandler = default!;
        private Stopwatch _stopwatch = default!;
        protected double _totalSeconds;
        protected double _headersSeconds;
        protected List<T> _data = new List<T>();
        protected int _collectionCount = 100;
        protected bool _loadingData = false;
        protected bool _useGrpc = false;
        protected string _searchTerm = string.Empty;


        protected override async Task OnInitializedAsync()
        {
            _searchTerm = SearchTerm;
            _stopwatch = new Stopwatch();
            _httpHandler = new BenchmarkHttpHandler(_stopwatch, new HttpClientHandler());
            _httpClient = new HttpClient(_httpHandler)
            {
                BaseAddress = new Uri(Configuration.GetSection("api").GetValue<string>("baseUrl"))
            };
            _stateSubscription = ComponentState?.RegisterOnPersisting(PersistDataAsync);
            if (ComponentState != null && ComponentState.TryTakeFromJson<List<T>>($"DataFor{typeof(T).Name}", out var restored))
            {
                Console.WriteLine($"Loaded data from Componentstate (Count: {restored?.Count ?? 0})");
                _data = restored ?? new List<T>();
            }
            else
            {
                // Meldung aktuallisieren wenn noch kein Pre-Rendering statt gefunden hat.
                Console.WriteLine("Load data from API.");
                await LoadData();
            }
            await base.OnInitializedAsync();
        }

        protected async Task OnCollectionCountChanged(int collectionCount)
        {
            _collectionCount = collectionCount;
            await LoadData();
        }

        protected async Task OnSearchTermChanged(string searchTerm)
        {
            _searchTerm = searchTerm;
            await LoadData();
        }

        protected async Task OnUseGrpcChanged(bool useGrpc)
        {
            _useGrpc = useGrpc;
            await LoadData();
        }

        protected async Task LoadData()
        {
            _loadingData = true;
            try
            {
                _stopwatch.Start();
                _data = _useGrpc ? await LoadDataFromGrpcService(_collectionCount, _searchTerm) : await LoadDataFromWebApi(_collectionCount, _searchTerm);
                _headersSeconds = _httpHandler.HeadersReceivedElapsed!.Value.TotalSeconds;
                _totalSeconds = _stopwatch.Elapsed.TotalSeconds;
                _loadingData = false;
            }
            catch (Exception ex)
            {
                var currentType = typeof(T);
                Console.WriteLine($"Load data for {currentType.Name} failed. Error: {ex.Message}");
                throw new ArgumentException($"No data found for {currentType.Name}.");
            }
            finally
            {
                _stopwatch.Reset();
            }
        }

        protected abstract Task<List<T>> LoadDataFromGrpcService(int count, string searchTerm);

        private async Task<List<T>> LoadDataFromWebApi(int count, string searchTerm)
        {
            if (_httpClient != null)
            {
                var url = String.IsNullOrWhiteSpace(searchTerm) 
                    ? $"{typeof(T).Name}?take={count}" 
                    : $"{typeof(T).Name}/filter/{searchTerm}?take={count}";
                var data = await _httpClient.GetFromJsonAsync<List<T>>(url);
                return data ?? new List<T>();
            }
            return new List<T>();
        }

        private Task PersistDataAsync()
        {
            Console.WriteLine($"Persist current data for conferences");
            ComponentState?.PersistAsJson($"DataFor{typeof(T).Name}", _data);
            return Task.CompletedTask;
        }

        public void Dispose() => _stateSubscription?.Dispose();
    }
}
