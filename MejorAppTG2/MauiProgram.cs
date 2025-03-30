using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using epj.CircularGauge.Maui;
using Material.Components.Maui.Extensions;
using epj.Expander.Maui;

namespace MejorAppTG2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {

            var builder = MauiApp.CreateBuilder();
            builder

            .UseMauiApp<App>()
            // Initialize the .NET MAUI Community Toolkit by adding the below line of code
            .UseMauiCommunityToolkit()
            .UseExpander() // optional: add this
           // Inicializo esto
             .UseCircularGauge() // add this line
            // Añadir material design
            .UseMaterialComponents()
            // After initializing the .NET MAUI Community Toolkit, optionally add additional fonts
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                // Fuentes personalizadas que usaremos

                fonts.AddFont("SF.OTF", "SanFrancisco");

                Expander.EnableAnimations();


            });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
