using Microsoft.AspNetCore.Components;
using Net6Features.Shared.Models;
using System.Net.Http.Json;

namespace Net6Features.Client.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient = default!;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient??throw new ArgumentNullException(nameof(httpClient));
        }

        public Task<Temperatures?> GetCurrentWeather(string city)
        {
            return _httpClient.GetFromJsonAsync<Temperatures>($"https://api.openweathermap.org/data/2.5/weather?q={city}&lang=de&units=metric&appid=4ca4543651e1412e4ff90a4593c7807e");
        }
    }
}
