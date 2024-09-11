using Microsoft.SemanticKernel;
using System.ComponentModel;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace Hackathon_Neworbit;

public class TmdbPlugin
{
    private readonly TMDbClient tmdbClient;

    public TmdbPlugin(TMDbLib.Client.TMDbClient tmdbClient)
    {
        this.tmdbClient = tmdbClient;
    }

    [KernelFunction("get_collection")]
    [Description("Search for movie collection metadata")]
    [return: Description("Closest matching movie collection")]
    public async Task<SearchCollection?> SearchMovieCollection(string searchTerm)
    {
        ChatPrinter.PrintDependency("- Calling movie collection lookup -");
        var collections = await tmdbClient.SearchCollectionAsync(searchTerm);
        return collections.Results.First();
    }

    [KernelFunction("get_movies_in_collection")]
    [Description("List movies in a collection")]
    [return: Description("the list of movies")]
    public async Task<List<SearchMovie>> GetMoviesInCollection(int collectionId)
    {
        ChatPrinter.PrintDependency("- Calling movies in collection lookup -");
        var collection = await tmdbClient.GetCollectionAsync(collectionId);
        return collection.Parts;
    }

    [KernelFunction("get_movie")]
    [Description("Get movie details by id")]
    [return: Description("Movie data")]
    public async Task<Movie?> GetMovie(int movieId)
    {
        ChatPrinter.PrintDependency("- Calling movie lookup -");
        var movie = await tmdbClient.GetMovieAsync(movieId);
        return movie;
    }
}
