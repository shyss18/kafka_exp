namespace KP.Worker.Models;

internal record WeatherForecast
{
    public int Id { get; set; }
    
    public DateTime Date { get; }
    
    public string City { get; }

    public double TemperatureInCelsius { get; }

    public double TemperatureInFahrenheit => TemperatureInCelsius * 1.8 + 32;

    public WeatherForecast(string city, double temperatureInCelsius, DateTime date)
    {
        City = city;
        TemperatureInCelsius = temperatureInCelsius;
        Date = date;
    }
}