using DevExpress.Blazor;
using EDO.WorkFlow.Data;
using EDO.WorkFlow.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace EDO.WorkFlow
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
		//builder.Logging.AddDebug();
#endif
            builder.Services.AddAuthorizationCore(); // This is the core functionality  

            builder.Services.AddScoped<CustomAuthenticationStateProvider>(); // This is our custom provider
                                                                             //When asking for the default Microsoft one, give ours!  
            builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomAuthenticationStateProvider>());

            builder.Services.AddBlazorWebView();
            builder.Services.AddI18nText();

            builder.Services.AddSingleton<IDocumentService, DocumentService>();
            builder.Services.AddSingleton<WeatherForecastService>();

            builder.Services.AddDevExpressBlazor(configure => configure.BootstrapVersion = BootstrapVersion.v5);

            return builder.Build();
        }
    }
}