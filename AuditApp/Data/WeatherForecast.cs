using System.ComponentModel.DataAnnotations;

namespace AuditApp.Data
{
    public class WeatherForecast
    {
        [Key]
        [Required]
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}
