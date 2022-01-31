using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Net6Features.Client.Services;
using Net6Features.Shared.Models;

namespace Net6Features.Client.Pages
{
    public partial class Index
    {
        [Inject] public PersistentComponentState? ComponentState { get; set; }
        [Inject] public NavigationManager? NavigationManager { get; set; }
        [Inject] public WeatherService WeatherService { get; set; } = default!;
        
        private string _pageTitle = ".NET 6";
        private string _description = "Neue Features coole in .NET 6";
        private string _dataSource = string.Empty;
        private string _searchTerm = string.Empty;
        private string _imgPath = string.Empty;
        private Temperatures? _temperatures;
        private PersistingComponentStateSubscription? _stateSubscription;

        protected override async Task OnInitializedAsync()
        {
            _stateSubscription = ComponentState?.RegisterOnPersisting(PersistDataAsync);
            if (ComponentState != null && ComponentState.TryTakeFromJson<Temperatures>($"CurrentWeather", out var restored))
            {
                Console.WriteLine($"Loaded data from Componentstate");
                _temperatures = restored;
            }
            else
            {
                // Meldung aktuallisieren wenn noch kein Pre-Rendering statt gefunden hat.
                Console.WriteLine("Load data from Api.");
                _temperatures = await WeatherService.GetCurrentWeather("Rodalben");
            }

            _imgPath = $"http://openweathermap.org/img/wn/{_temperatures?.Weather.FirstOrDefault()?.Icon ?? "03d"}@2x.png";
            await base.OnInitializedAsync();
        }

        private Task PersistDataAsync()
        {
            Console.WriteLine($"Persist current data for conferences");
            ComponentState?.PersistAsJson($"CurrentWeather", _temperatures);
            return Task.CompletedTask;
        }


        public void NavigateToDataSource()
        {
            if (NavigationManager is not null && !String.IsNullOrWhiteSpace(_dataSource))
            {
                var url = string.IsNullOrWhiteSpace(_searchTerm) 
                    ? $"{_dataSource}" 
                    : $"{_dataSource}?searchTerm={_searchTerm}";
                NavigationManager.NavigateTo(url);
            }
        }
        private void OnSelectedOptionChanged(string option)
        {
            _dataSource = option;
        }
        public void Dispose() => _stateSubscription?.Dispose();
    }
}