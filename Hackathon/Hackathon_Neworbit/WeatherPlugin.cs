using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

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
        return "The weather in " + location + " is sunny.";
    }
}