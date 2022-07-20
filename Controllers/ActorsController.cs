using Microsoft.AspNetCore.Mvc;
using MoviePro.Services.Interfaces;

namespace MoviePro.Controllers;

public class ActorsController : Controller
{
    private readonly IRemoteMovieService _tmdbMovieService;
    private readonly IDataMappingService _tmdbMappingService;

    public ActorsController(IRemoteMovieService tmdbMovieService, IDataMappingService mappingService)
    {
        _tmdbMovieService = tmdbMovieService;
        _tmdbMappingService = mappingService;
    }


    public async Task<IActionResult> Details(int id)
    {
        var actor = await _tmdbMovieService.ActorDetailAsync(id);
        actor = _tmdbMappingService.MapActorDetail(actor);
        return View(actor);
    }
}
