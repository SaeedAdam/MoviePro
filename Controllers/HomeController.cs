using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviePro.Data;
using MoviePro.Enums;
using MoviePro.Models;
using MoviePro.Models.ViewModels;
using MoviePro.Services.Interfaces;
using System.Diagnostics;

namespace MoviePro.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IRemoteMovieService _tmdbMovieService;

    public HomeController(ILogger<HomeController> logger, IRemoteMovieService tmdbMovieService, ApplicationDbContext context)
    {
        _logger = logger;
        _tmdbMovieService = tmdbMovieService;
        _context = context;
    }

    public async Task<IActionResult> Index()

    {
        const int count = 16;
        var data = new LandingPageVM()
        {
            CustomCollections = await _context.Collection
                .Include(c => c.MovieCollections)
                .ThenInclude(mc => mc.Movie)
                .ToListAsync(),
            NowPlaying = await _tmdbMovieService.MovieSearchAsync(MovieCategory.now_playing, count),
            Popular = await _tmdbMovieService.MovieSearchAsync(MovieCategory.popular, count),
            TopRated = await _tmdbMovieService.MovieSearchAsync(MovieCategory.top_rated, count),
            UpComing = await _tmdbMovieService.MovieSearchAsync(MovieCategory.upcoming, count)
        };

        return View(data);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
