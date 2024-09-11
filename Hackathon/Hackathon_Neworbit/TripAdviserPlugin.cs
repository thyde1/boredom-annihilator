using Hackathon_Neworbit;
using Microsoft.SemanticKernel;
using System.ComponentModel;

public class TripAdvisorPlugin
{
    public TripAdvisorPlugin()
    {
    }

    [KernelFunction("get_locations")]
    [Description("Get the current and future weather forecast")]
    [return: Description("The weather data.")]
    public string GetWeather(string location)
    {
        ChatPrinter.PrintDependency("- Calling weather lookup -");
        return "The weather in " + location + " is sunny.";
    }
}