using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MoviePro.Data;
using MoviePro.Models.Database;
using MoviePro.Models.Settings;


namespace MoviePro.Services;

public class SeedService
{
    private readonly AppSettings _appSettings;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public SeedService(IOptions<AppSettings> appSettings, UserManager<IdentityUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _context = context;
        _roleManager = roleManager;
        _appSettings = appSettings.Value;
    }

    public async Task ManageDataAsync()
    {
        await UpdateDatabaseAsync();

        await SeedRoleAsync();

        await SeedUserAsync();

        await SeedCollection();
    }

    private async Task UpdateDatabaseAsync()
    {
        await _context.Database.MigrateAsync();
    }

    private async Task SeedRoleAsync()
    {
        if (_context.Roles.Any())
        {
            return;
        }

        var adminRole = _appSettings.MovieProSettings.DefaultCredentials.Role;

        await _roleManager.CreateAsync(new IdentityRole(adminRole));

    }

    private async Task SeedUserAsync()
    {
        if (_userManager.Users.Any()) return;

        var credentials = _appSettings.MovieProSettings.DefaultCredentials;
        var newUser = new IdentityUser()
        {
            Email = credentials.Email,
            UserName = credentials.Email,
            EmailConfirmed = true
        };

        await _userManager.CreateAsync(newUser, credentials.Password);
        await _userManager.AddToRoleAsync(newUser, credentials.Role);
    }

    private async Task SeedCollection()
    {
        if (_context.Collection.Any()) return;

        _context.Add(new Collection()
        {
            Name = _appSettings.MovieProSettings.DefaultCollection.Name,
            Description = _appSettings.MovieProSettings.DefaultCollection.Description
        });

        await _context.SaveChangesAsync();
    }
}
