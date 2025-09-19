using Microsoft.EntityFrameworkCore;
using AuditApp.Data;
using Microsoft.AspNetCore.Http.HttpResults;
namespace AuditApp.Controllers;

public static class WeatherForecastEndpoints
{
    public static void MapWeatherForecastEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/WeatherForecast").WithTags(nameof(WeatherForecast));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.WeatherForecast.ToListAsync();
        })
        .WithName("GetAllWeatherForecasts")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<WeatherForecast>, NotFound>> (DateOnly date, AppDbContext db) =>
        {
            return await db.WeatherForecast.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Date == date)
                is WeatherForecast model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetWeatherForecastById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (DateOnly date, WeatherForecast weatherForecast, AppDbContext db) =>
        {
            var affected = await db.WeatherForecast
                .Where(model => model.Date == date)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Date, weatherForecast.Date)
                    .SetProperty(m => m.TemperatureC, weatherForecast.TemperatureC)
                    .SetProperty(m => m.Summary, weatherForecast.Summary)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateWeatherForecast")
        .WithOpenApi();

        group.MapPost("/", async (WeatherForecast weatherForecast, AppDbContext db) =>
        {
            db.WeatherForecast.Add(weatherForecast);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/WeatherForecast/{weatherForecast.Date}",weatherForecast);
        })
        .WithName("CreateWeatherForecast")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (DateOnly date, AppDbContext db) =>
        {
            var affected = await db.WeatherForecast
                .Where(model => model.Date == date)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteWeatherForecast")
        .WithOpenApi();
    }
}
