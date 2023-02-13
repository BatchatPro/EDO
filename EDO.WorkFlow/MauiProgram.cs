﻿using EDO.WorkFlow.Data;
using EDO.WorkFlow.Services;
using Microsoft.Extensions.Logging;
//using DevExpress.Blazor;

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
		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IDocumentService, DocumentService>();
            builder.Services.AddSingleton<WeatherForecastService>();
            //builder.Services.AddDevExpressBlazor(configure => configure.BootstrapVersion = BootstrapVersion.v5);

            return builder.Build();
        }
    }
}