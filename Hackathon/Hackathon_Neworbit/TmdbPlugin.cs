using Hackathon_Neworbit.Tmdb;
using Microsoft.SemanticKernel;
using System.ComponentModel;
using TMDbLib.Client;
using TMDbLib.Objects.General;
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

    [KernelFunction("get_genres")]
    [Description("Get list of movie genres")]
    [return: Description("list of movie genres")]
    public async Task<List<Genre>> GetMovieGenres()
    {
        ChatPrinter.PrintDependency("- Calling get movie genres lookup -");
        var genres = await tmdbClient.GetMovieGenresAsync();
        return genres;
    }

    [KernelFunction("get_movies_in_genres")]
    [Description("Get list of movies in in the given genres. The genres will be ORed")]
    [return: Description("list of movies")]
    public async Task<List<MovieDto>> GetMoviesInGenre(int[] genreIds)
    {
        ChatPrinter.PrintDependency($"- Calling get movies in genres lookup {string.Join(", ", genreIds)} -");
        var movies = await tmdbClient.DiscoverMoviesAsync()
            .IncludeWithAnyOfGenre(genreIds)
            .Query();
            return movies.Results.Select(m => new MovieDto
            {
                Title = m.Title,
                Overview = m.Overview
            }).ToList();
    }
}
