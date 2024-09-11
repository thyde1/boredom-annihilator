using Hackathon_Neworbit;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using System.Collections.Specialized;
using System.ComponentModel;

public class GeoApifyPlugin
{
    private readonly HttpClient httpClient;
    private readonly IConfiguration configuration;

    public GeoApifyPlugin(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        this.httpClient = httpClientFactory.CreateClient();
        this.configuration = configuration;
    }

    [KernelFunction("get_location")]
    [Description("Get location id for a place")]
    [return: Description("Location id for a place with a given name")]
    public async Task<string> GetLocationId(string placeName)
    {
        ChatPrinter.PrintDependency($"- Calling Locations lookup ({placeName}) -");
        var apiKey = this.configuration.GetValue<string>("geoApifyApiKey");
        var uriBuilder = new UriBuilder("https://api.geoapify.com/v1/geocode/autocomplete");
        NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
        queryString.Add("apiKey", apiKey);
        queryString.Add("text", placeName);
        uriBuilder.Query = queryString.ToString();
        var response = await this.httpClient.GetAsync(uriBuilder.Uri);
        var data = await response.Content.ReadAsStringAsync();
        return data;
    }

    [KernelFunction("get_attractions")]
    [Description("Get tourist attractions")]
    [return: Description("Details of matching tourist attractions.")]
    public async Task<string> GetAttractions(string locationId)
    {
        ChatPrinter.PrintDependency($"- Calling Attractions lookup {locationId} -");
        var apiKey = this.configuration.GetValue<string>("geoApifyApiKey");
        var uriBuilder = new UriBuilder("https://api.geoapify.com/v2/places");
        NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
        queryString.Add("apiKey", apiKey);
        queryString.Add("filter", $"place:{locationId}");
        queryString.Add("categories", "tourism");
        queryString.Add("limit", "3");
        uriBuilder.Query = queryString.ToString();
        var response = await this.httpClient.GetAsync(uriBuilder.Uri);
        var data = await response.Content.ReadAsStringAsync();
        return data;
    }
}