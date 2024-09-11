using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using System.Collections.Specialized;
using System.ComponentModel;

public class WeatherPlugin
{
    private readonly HttpClient httpClient;
    private readonly IConfiguration configuration;

    public WeatherPlugin(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        this.httpClient = httpClientFactory.CreateClient();
        this.configuration = configuration;
    }

    [KernelFunction("get_weather")]
    [Description("Get the current and future weather forecast")]
    [return: Description("The weather data.")]
    public async Task<string> GetWeather(string location)
    {
        var uriBuilder = new UriBuilder("https://api.openweathermap.org/data/3.0/onecall");
        NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
        queryString.Add("lat", "51.677556");
        queryString.Add("lon", "-1.126336");
        queryString.Add("exclude", "minutely,alerts");
        queryString.Add("appid", configuration.GetValue<string>("weatherKey"));
        uriBuilder.Query = queryString.ToString();

        var response = await httpClient.GetAsync(uriBuilder.Uri);

        var data = await response.Content.ReadAsStringAsync();
        return data;
    }
}