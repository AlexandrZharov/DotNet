using Microsoft.EntityFrameworkCore;

namespace AuditApp.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<WeatherForecast> WeatherForecast { get; set; }
    }
}
