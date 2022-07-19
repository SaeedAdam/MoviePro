using MoviePro.Enums;
using MoviePro.Models.TMDB;

namespace MoviePro.Services.Interfaces;

public interface IRemoteMovieService
{
    Task<MovieDetail> MovieDetailAsync(int id);
    Task<MovieSearch> MovieSearchAsync(MovieCategory category, int count);
    Task<ActorDetail> ActorDetailAsync(int id);
}
