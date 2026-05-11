using FrontendApp.Models;
using FrontendApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FrontendApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly WeatherApiService _weatherApiService;

    public HomeController(
        ILogger<HomeController> logger,
        WeatherApiService weatherApiService)
    {
        _logger = logger;
        _weatherApiService = weatherApiService;
    }

    public async Task<IActionResult> Index()
    {
        var weatherData = await _weatherApiService.GetWeatherForecastAsync();

        return View(weatherData);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}