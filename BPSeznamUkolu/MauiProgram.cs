using BPSeznamUkolu.Data;
using BPSeznamUkolu.Services;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace BPSeznamUkolu
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts => {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "checklist.db");
            var connectionString = $"Data Source={dbPath}";

            builder.Services.AddDbContextFactory<AppDbContext>(options => options.UseSqlite(connectionString));
            builder.Services.AddScoped<IDatabaseService, DatabaseService>();

            var app = builder.Build();
            using var scope = app.Services.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(MauiProgram));
            var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();

            try {
                using var dbContext = dbContextFactory.CreateDbContext();
                dbContext.Database.Migrate();
            }
            catch (Exception ex) 
            {
                logger.LogError(ex, "An error occurred while migrating the database.");
            }

            return app;
        }
    }
}
