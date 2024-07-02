using KP.Worker.Models;

namespace KP.Worker.Generator;

internal static class WeatherGenerator
{
    private static string[] Cities => new[] { "Moscow", "New York", "London", "Minsk", "Mogilev", "Warsaw" }; 
    
    public static IEnumerable<WeatherForecast> Generate()
    {
        var randomStart = new Random().Next(1, 10);
        var randomCount = new Random().Next(1, 10);
        
        var cityIndexRandomizer = new Random();
        
        var date = DateTime.UtcNow;

        return Enumerable.Range(randomStart, randomCount)
            .Select(item => new WeatherForecast(Cities[cityIndexRandomizer.Next(0, Cities.Length)], item * 11 ,date));
    }
}