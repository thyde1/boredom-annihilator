using Microsoft.SemanticKernel;
using System.ComponentModel;

public class WeatherPlugin
{
    public WeatherPlugin()
    {
    }

    [KernelFunction("get_weather")]
    [Description("Get the current and future weather forecast")]
    [return: Description("The weather data.")]
    public string GetWeather(string location)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("- Calling weather lookup -");
        Console.ForegroundColor = ConsoleColor.Gray;
        return "The weather in " + location + " is sunny.";
    }
}