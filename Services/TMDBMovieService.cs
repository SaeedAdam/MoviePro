using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using MoviePro.Enums;
using MoviePro.Models.Settings;
using MoviePro.Models.TMDB;
using MoviePro.Services.Interfaces;

namespace MoviePro.Services;

public class TMDBMovieService : IRemoteMovieService
{
    private readonly AppSettings _appSettings;
    private readonly IHttpClientFactory _httpClientFactory;
    public TMDBMovieService(IOptions<AppSettings> appSettings, IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _appSettings = appSettings.Value;
    }


    public async Task<MovieSearch> MovieSearchAsync(MovieCategory category, int count)
    {
        //Step 1: Setup a default instance of MovieSearch
        MovieSearch movieSearch = new();


        //Step 2: Assemble the full request uri string
        var query = $"{_appSettings.TMDBSettings.BaseUrl}/movie/{category}";

        var queryParams = new Dictionary<string, string>
        {
            {"api_key", _appSettings.MovieProSettings.TmDbApiKey},
            {"language", _appSettings.TMDBSettings.QueryOptions.Language},
            {"page", _appSettings.TMDBSettings.QueryOptions.Page}
        };

        var requestUri = QueryHelpers.AddQueryString(query, queryParams);


        // Step 3: Create a client and execute the request
        var client = _httpClientFactory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        var response = await client.SendAsync(request);


        //Step 4: Return the MovieSearch object
        if (response.IsSuccessStatusCode)
        {
            var dcjs = new DataContractJsonSerializer(typeof(MovieSearch));
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            movieSearch = (MovieSearch) dcjs.ReadObject(responseStream);
            movieSearch.results = movieSearch.results.Take(count).ToArray();
            movieSearch.results.ToList()
                .ForEach(r =>
                    r.poster_path = $"{_appSettings.TMDBSettings.BaseImagePath}/{_appSettings.MovieProSettings.DefaultPosterSize}/{r.poster_path}");
        }

        return movieSearch;
    }

    public async Task<MovieDetail> MovieDetailAsync(int id)
    {
        //Step 1: Setup a default return object
        MovieDetail movieDetail = new();


        //Step 2: Assemble the full request uri string
        var query = $"{_appSettings.TMDBSettings.BaseUrl}/movie/{id}";

        var queryParams = new Dictionary<string, string>
        {
            {"api_key", _appSettings.MovieProSettings.TmDbApiKey},
            {"language", _appSettings.TMDBSettings.QueryOptions.Language},
            {"append_to_response", _appSettings.TMDBSettings.QueryOptions.AppendToResponse}
        };

        var requestUri = QueryHelpers.AddQueryString(query, queryParams);


        // Step 3: Create a client and execute the request
        var client = _httpClientFactory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        var response = await client.SendAsync(request);


        //Step 4: Deserialize into MovieDetail
        if (response.IsSuccessStatusCode)
        {
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var dcjs = new DataContractJsonSerializer(typeof(MovieDetail));
            movieDetail = dcjs.ReadObject(responseStream) as MovieDetail;
        }

        return movieDetail;
    }

    

    public async Task<ActorDetail> ActorDetailAsync(int id)
    {
        //Step 1: Setup a default return object
        ActorDetail actorDetail = new();


        //Step 2: Assemble the full request uri string
        var query = $"{_appSettings.TMDBSettings.BaseUrl}/person/{id}";

        var queryParams = new Dictionary<string, string>
        {
            {"api_key", _appSettings.MovieProSettings.TmDbApiKey},
            {"language", _appSettings.TMDBSettings.QueryOptions.Language}
        };

        var requestUri = QueryHelpers.AddQueryString(query, queryParams);


        // Step 3: Create a client and execute the request
        var client = _httpClientFactory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        var response = await client.SendAsync(request);


        //Step 4: Deserialize into ACtorDetail object
        if (response.IsSuccessStatusCode)
        {
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var dcjs = new DataContractJsonSerializer(typeof(ActorDetail));
            actorDetail = (ActorDetail) dcjs.ReadObject(responseStream);
        }

        return actorDetail;

    }
}
