namespace KC.Worker.Models;

internal record WeatherForecast
{
    public DateTime Date { get; }
    
    public string City { get; }

    public double TemperatureInCelsius { get; }

    public double TemperatureInFahrenheit { get; }

    public WeatherForecast(string city, double temperatureInCelsius, DateTime date, double temperatureInFahrenheit)
    {
        City = city;
        TemperatureInCelsius = temperatureInCelsius;
        Date = date;
        TemperatureInFahrenheit = temperatureInFahrenheit;
    }
}