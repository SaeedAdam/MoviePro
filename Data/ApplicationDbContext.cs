using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MoviePro.Models.Database;

namespace MoviePro.Data;
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }


    public DbSet<Collection> Collection { get; set; }
    public DbSet<Movie> Movie { get; set; }
    public DbSet<MovieCollection> MovieCollection { get; set; }
    public DbSet<MovieCast> Cast { get; set; }
    public DbSet<MovieCrew> Crew { get; set; }
}
