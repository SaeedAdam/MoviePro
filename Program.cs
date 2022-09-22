using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MoviePro.Data;
using MoviePro.Models.Settings;
using MoviePro.Services;
using MoviePro.Services.Interfaces;

namespace MoviePro;
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = ConnectionService.GetConnectionString(builder.Configuration);

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        // Register and inject an IOptions of type AppSettings model to easily access the configuration
        // settings as a strongly typed object
        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

        builder.Services.AddScoped<AppSettings>();

        // Register our SeedService
        builder.Services.AddTransient<SeedService>();

        // Register HttpClient since is used by TMDBMovieService
        builder.Services.AddHttpClient();

        // Register our TMDBMovieService
        builder.Services.AddScoped<IRemoteMovieService, TMDBMovieService>();

        // Register our TMDBMappingService
        builder.Services.AddScoped<IDataMappingService, TMDBMappingService>();

        // Register our BasicImageService
        builder.Services.AddSingleton<IImageService, BasicImageService>();

        // Register and inject an IOptions of type MailSettings model to easily access the configuration
        // settings as a strongly typed object
        //builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

        //// Register our EmailService
        //builder.Services.AddScoped<IMovieEmailSender, EmailService>();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
            app.UseDeveloperExceptionPage();
        }
        else
        {
            // Use redirect to custom 404 error page
            app.UseStatusCodePagesWithReExecute("/Home/HandleError/{0}");

            app.UseExceptionHandler("/Home/Error");

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        // Enable automatic tracing integration.
        // If running with .NET 5 or below, make sure to put this middleware
        // right after `UseRouting()`.
        //app.UseSentryTracing();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        // Create instance of our SeedService and call initial migration
        var dataService = app.Services.CreateScope().ServiceProvider.GetRequiredService<SeedService>();
        await dataService.ManageDataAsync();

        // To test if Sentry is correctly configure run the following command
        //SentrySdk.CaptureMessage("Hello Sentry, MoviePro application launched");

        app.Run();
    }
}
