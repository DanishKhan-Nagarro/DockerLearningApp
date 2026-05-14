using BackendApi.Data;
using BackendApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(
    options =>
    {
        options.UseSqlite("Data Source=tasks.db");
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var databaseReady = false;
    var retryCount = 0;

while (!databaseReady && retryCount < 10)
{
    try
    {
        db.Database.EnsureCreated();

        databaseReady = true;
    }
    catch
    {
        retryCount++;

        await Task.Delay(3000);
    }
}
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

//Tasks Endpoints for both Get and Post

app.MapGet(
    "/api/tasks",
    async (AppDbContext db) =>
    {
        return await db.Tasks.ToListAsync();
    });

app.MapPost(
    "/api/tasks",
    async (TaskItem task,AppDbContext db) =>
    {
        if (string.IsNullOrWhiteSpace(task.Title))
        {
            return Results.BadRequest("Task title is required.");
        }

        db.Tasks.Add(task);

        await db.SaveChangesAsync();

        return Results.Created($"/api/tasks/{task.Id}", task);
    });

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
