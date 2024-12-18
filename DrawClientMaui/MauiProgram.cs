﻿using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
// using CommunityToolkit.Maui;

namespace DrawClientMaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                // .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MaterialIcons-Regular.ttf", "GoogleMaterialFont");
                    fonts.AddFont("HennyPenny-Regular.ttf", "HennyPenny");
                    fonts.AddFont("Roboto-Regular.ttf", "Roboto");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
