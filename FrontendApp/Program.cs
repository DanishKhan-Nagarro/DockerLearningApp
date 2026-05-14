using FrontendApp.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var backendApiUrl =
    builder.Configuration["BackendApiUrl"]
    ?? "http://localhost:5102/";

builder.Services.AddHttpClient<WeatherApiService>(
    client =>
    {
        client.BaseAddress =
            new Uri(backendApiUrl);
    });

builder.Services.AddHttpClient(
    "BackendApi",
    client =>
    {
        client.BaseAddress =
            new Uri(backendApiUrl);
    });
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
