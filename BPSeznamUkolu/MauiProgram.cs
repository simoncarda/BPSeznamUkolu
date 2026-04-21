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
            builder.Services.AddDbContext<AppDbContext>();
            builder.Services.AddScoped<IDatabaseService, DatabaseService>();

            var app = builder.Build();
            using (var scope = app.Services.CreateScope()) {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                try {
                    dbContext.Database.Migrate();
                }
                catch (Exception ex) {
                    Console.WriteLine($"Chyba při migraci databáze: {ex.Message}");
                }
            }

            return app;
        }
    }
}
