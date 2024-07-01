using KC.Worker.Models;

namespace KC.Worker.DataProcessor;

internal static class WeatherProcessor
{
    public static string Process(WeatherForecast weatherForecast)
        => $"City: {weatherForecast.City}; Date: {weatherForecast.Date:F}; Celsius: {weatherForecast.TemperatureInCelsius}; Fahrenheit : {weatherForecast.TemperatureInFahrenheit}";
}