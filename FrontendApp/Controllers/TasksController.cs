using FrontendApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FrontendApp.Controllers;

public class TasksController : Controller
{
    private readonly HttpClient _httpClient;

    public TasksController(
        IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("BackendApi");
    }

    public async Task<IActionResult> Index()
    {
        var response =
            await _httpClient.GetAsync(
                "/api/tasks");

        var tasks = new List<TaskItem>();

        if (response.IsSuccessStatusCode)
        {
            var json =
                await response.Content
                    .ReadAsStringAsync();

            tasks =
                JsonSerializer.Deserialize<
                    List<TaskItem>>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? [];
        }

        return View(tasks);
    }

    [HttpPost]
    public async Task<IActionResult> AddTask(
        string title)
    {
        if (!string.IsNullOrWhiteSpace(title))
        {
            var task = new TaskItem
            {
                Title = title
            };

            await _httpClient.PostAsJsonAsync(
                "/api/tasks",
                task);
        }

        return RedirectToAction(nameof(Index));
    }
}