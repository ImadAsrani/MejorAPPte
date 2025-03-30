using MejorAppTG2.Vista;
using System.Globalization;

namespace MejorAppTG2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registro de rutas
            Routing.RegisterRoute(nameof(FoodTest), typeof(Vista.FoodTest));
            Routing.RegisterRoute(nameof(LongTest), typeof(Vista.LongTest));
            Routing.RegisterRoute(nameof(ShortTest), typeof(Vista.ShortTest));
            Routing.RegisterRoute(nameof(ResultPage), typeof(Vista.ResultPage));
        }
    }

}
