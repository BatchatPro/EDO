﻿using Microsoft.AspNetCore.Components.Authorization;
using WorkFlowApp.Data;
using WorkFlowApp.Services;

namespace WorkFlowApp
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
            builder.Services.AddScoped(x => new HttpClient());
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            //builder.Logging.AddDebug();
#endif
            builder.Services.AddAuthorizationCore(); // This is the core functionality
            builder.Services.AddScoped<CustomAuthenticationStateProvider>(); // This is our custom provider
                                                                             //When asking for the default Microsoft one, give ours!
            builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomAuthenticationStateProvider>());

            builder.Services.AddBlazorWebView();
            
            builder.Services.AddScoped<ILoginService, LoginService>();

            builder.Services.AddSingleton<IDocumentService, DocumentService>();
            builder.Services.AddSingleton<WeatherForecastService>();

            return builder.Build();
        }
    }
}