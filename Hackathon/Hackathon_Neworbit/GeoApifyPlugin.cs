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

    [KernelFunction("get_attractions")]
    [Description("Get tourist attractions")]
    [return: Description("Details of matching tourist attractions.")]
    public async Task<string> GetAttractions()
    {
        ChatPrinter.PrintDependency("- Calling Attractions lookup -");
        var apiKey = this.configuration.GetValue<string>("geoApifyApiKey");
        var uriBuilder = new UriBuilder("https://api.geoapify.com/v2/places");
        NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
        queryString.Add("apiKey", apiKey);
        queryString.Add("filter", "place:51df4e22c2bf88f6bf59fdc1c073ef454a40c0020b920307456e676c616e64e2032177686f736f6e66697273743a6d6163726f726567696f6e3a343034323237343639");
        queryString.Add("categories", "tourism");
        queryString.Add("limit", "3");
        uriBuilder.Query = queryString.ToString();
        var response = await this.httpClient.GetAsync(uriBuilder.Uri);
        var data = await response.Content.ReadAsStringAsync();
        return data;
    }
}