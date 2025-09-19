namespace AuditApp.Data
{
    public static class InMemoryDatabaseContentSetup
    {
        private static string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

        public static void AddInMemoryDatabaseContent(this WebApplication app)
        {
            var databaseScope = app.Services.CreateScope();
            var context = databaseScope
                .ServiceProvider
                .GetRequiredService<AppDbContext>();

            Enumerable.Range(1, 5).Select(index =>
                context.Add(new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = summaries[Random.Shared.Next(summaries.Length)]
                }))
            .ToArray();
            context.SaveChanges();
        }
    }
}
