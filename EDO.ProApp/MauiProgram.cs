using EDO.ProApp.Data;
using Microsoft.Extensions.Logging;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace EDO.ProApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
            builder.Services.AddI18nText();
            builder.Services.AddSingleton<WeatherForecastService>();

            return builder.Build();
        }
    }
}