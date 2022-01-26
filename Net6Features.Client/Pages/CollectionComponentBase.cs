using Microsoft.AspNetCore.Components;
using Net6Features.Client.Components;
using System.Diagnostics;
using System.Net.Http.Json;

namespace Net6Features.Client.Pages
{
    public abstract class CollectionComponentBase<T> : ComponentBase, IDisposable
        where T : class
    {
        [Inject] public PersistentComponentState? ComponentState { get; set; }
        [Inject] public HttpClient? HttpClient { get; set; }

        [Parameter, SupplyParameterFromQuery(Name = "searchTerm")]
        public string SearchTerm { get; set; } = string.Empty;

        private PersistingComponentStateSubscription? _stateSubscription;
        protected List<T> _data = new List<T>();
        protected int _collectionCount = 100;
        protected bool _loadingData = false;
        protected bool _useGrpc = false;
        protected string _searchTerm = string.Empty;


        protected override async Task OnInitializedAsync()
        {
            _searchTerm = SearchTerm;
            _stateSubscription = ComponentState?.RegisterOnPersisting(PersistDataAsync);
            if (ComponentState != null && ComponentState.TryTakeFromJson<List<T>>($"DataFor{typeof(T).Name}", out var restored))
            {
                Console.WriteLine($"Loaded data from Componentstate (Count: {restored?.Count ?? 0})");
                _data = restored ?? new List<T>();
            }
            else
            {
                // Meldung aktuallisieren wenn noch kein Pre-Rendering statt gefunden hat.
                Console.WriteLine("Restore data failed.");
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
                var timer = new Stopwatch();
                Console.WriteLine(_useGrpc ? "Start to fetch data from GrpcService" : "Start to fetch data from WebApi");
                timer.Start();
                _data = _useGrpc ? await LoadDataFromGrpcService(_collectionCount, _searchTerm) : await LoadDataFromWebApi(_collectionCount, _searchTerm);
                timer.Stop();
                Console.WriteLine($"Time in s: {Math.Round(timer.Elapsed.TotalSeconds, 2)}");
                _loadingData = false;
            }
            catch (Exception ex)
            {
                var currentType = typeof(T);
                Console.WriteLine($"Load data for {currentType.Name} failed. Error: {ex.Message}");
                throw new ArgumentException($"No data found for {currentType.Name}.");
            }
        }

        protected abstract Task<List<T>> LoadDataFromGrpcService(int count, string searchTerm);

        private async Task<List<T>> LoadDataFromWebApi(int count, string searchTerm)
        {
            if (HttpClient != null)
            {
                var url = String.IsNullOrWhiteSpace(searchTerm) 
                    ? $"{typeof(T).Name}?take={count}" 
                    : $"{typeof(T).Name}/filter/{searchTerm}?take={count}";
                var data = await HttpClient.GetFromJsonAsync<List<T>>(url);
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
