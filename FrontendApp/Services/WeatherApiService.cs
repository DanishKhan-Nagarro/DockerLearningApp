using System.Net.Http.Json;

namespace FrontendApp.Services;

public class WeatherApiService
{
    private readonly HttpClient _httpClient;

    public WeatherApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<WeatherForecast>?> GetWeatherForecastAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>(
            "/weatherforecast");
    }
}

public class WeatherForecast
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }
}