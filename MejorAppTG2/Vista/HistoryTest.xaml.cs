
using MejorAppTG2.Localization;
using epj.Expander.Maui;
using Material.Components.Maui;
using Microsoft.Maui.Controls.Shapes;

namespace MejorAppTG2.Vista
{
    public partial class HistoryTest : ContentPage
    {

        private int contador = 1;

        public HistoryTest()
        {
            InitializeComponent();

     
            cargarTest();

        }

        
        private async void cargarTest()
        {
            await Task.Delay(400);


            var tests = await App.Database.GetShortTestAsync();

            foreach (var test in tests)
            {

                TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();

                Grid grid = new Grid
                {
                    VerticalOptions = LayoutOptions.Start,
                    Padding = 5,
                    RowDefinitions =
        {
                new RowDefinition { Height = new GridLength(40) },
                new RowDefinition { Height = new GridLength(20) },
                new RowDefinition { Height = new GridLength(20) },
                new RowDefinition { Height = new GridLength(20) },
                new RowDefinition { Height = new GridLength(20) },
                new RowDefinition { Height = new GridLength(40) }

        }
                };


                grid.Add(new Rectangle
                {
                    Margin = new Thickness(230, 0, 0, 0),
                    BackgroundColor = Color.FromArgb("#176cfc"),
                    HeightRequest = 120,
                    Rotation = 20,
                    WidthRequest = 120
                }, 0, 5);

                grid.Add(new Rectangle
                {
                    Margin = new Thickness(40, 20, 0, 0),
                    BackgroundColor = Color.FromArgb("#176cfc"),
                    HeightRequest = 120,
                    Rotation = 150,
                    WidthRequest = 120
                }, 0, 5);

                grid.Add(new Label
                {
                    Padding = new Thickness(10, 10, 0, 0),
                    FontFamily = "SF.OTF",
                    FontSize = 14,
                    Text = string.Format(AppResources.TituloTARap),
                    TextColor = Colors.White
                }, 0, 0);

                grid.Add(new Label
                {
                    Padding = new Thickness(10, 0, 0, 0),
                    FontFamily = "SFL.OTF",
                    FontSize = 14,
                    Text = string.Format(AppResources.Nombre) + ": " + test.user_id,
                    TextColor = Colors.White
                }, 0, 1);

                grid.Add(new Label
                {
                    Padding = new Thickness(10, 0, 0, 0),
                    FontFamily = "SFL.OTF",
                    FontSize = 14,
                    Text = string.Format(AppResources.Sexo) + ": " + test.user_sex,
                    TextColor = Colors.White
                }, 0, 2);

                grid.Add(new Label
                {
                    Padding = new Thickness(10, 0, 0, 0),
                    FontFamily = "SFL.OTF",
                    FontSize = 14,
                    Text = string.Format(AppResources.Edad) + ": " + test.user_age,
                    TextColor = Colors.White
                }, 0, 3);
                grid.Add(new Label
                {
                    Padding = new Thickness(10, 0, 0, 0),
                    FontFamily = "SFL.OTF",
                    FontSize = 14,
                    Text = string.Format(AppResources.Realizado) + ": " + test.date_done,
                    TextColor = Colors.White
                }, 0, 4);

                Border border = new Border

                {


                    Margin = new Thickness(0, 0, 0, 5),
                    BackgroundColor = Color.FromArgb("#1c73ff"),
                    HeightRequest = 140,
                    StrokeShape = new RoundRectangle()
                    {
                        CornerRadius = new CornerRadius(15)
                    },
                    StrokeThickness = 0
            ,
                    Content = grid
                };


                tapGestureRecognizer.Tapped += (s, e) =>
                {
                    animarTestSeleccionado(s, e, test.user_id, test.user_sex, test.user_age, test.fact01, test.fact02, test.fact03, "s");
                };

                border.GestureRecognizers.Add(tapGestureRecognizer);

                border.TranslationY = 800;

                gridHistorial.Add(border, 0, contador);


                contador++;


            }

            var testsLargos = await App.Database.GetLongTestAsync();

            foreach (var test in testsLargos)
            {

                TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();

                Grid grid = new Grid
                {
                    VerticalOptions = LayoutOptions.Start,
                    Padding = 5,
                    RowDefinitions =
        {
                new RowDefinition { Height = new GridLength(40) },
                new RowDefinition { Height = new GridLength(20) },
                new RowDefinition { Height = new GridLength(20) },
                new RowDefinition { Height = new GridLength(20) },
                new RowDefinition { Height = new GridLength(20) },
                new RowDefinition { Height = new GridLength(40) }


        }
                };


                grid.Add(new Rectangle
                {
                    Margin = new Thickness(230, 0, 0, 0),
                    BackgroundColor = Color.FromArgb("#00b365"),
                    HeightRequest = 120,
                    Rotation = 20,
                    WidthRequest = 120
                }, 0, 5);

                grid.Add(new Rectangle
                {
                    Margin = new Thickness(40, 20, 0, 0),
                    BackgroundColor = Color.FromArgb("#00b365"),
                    HeightRequest = 120,
                    Rotation = 150,
                    WidthRequest = 120
                }, 0, 5);

                grid.Add(new Label
                {
                    Padding = new Thickness(10, 10, 0, 0),
                    FontFamily = "SF.OTF",
                    FontSize = 14,
                    Text = string.Format(AppResources.TituloTACompl),
                    TextColor = Colors.White
                }, 0, 0);

                grid.Add(new Label
                {
                    Padding = new Thickness(10, 0, 0, 0),
                    FontFamily = "SFL.OTF",
                    FontSize = 14,
                    Text = string.Format(AppResources.Nombre) + ": " + test.user_id,
                    TextColor = Colors.White
                }, 0, 1);

                grid.Add(new Label
                {
                    Padding = new Thickness(10, 0, 0, 0),
                    FontFamily = "SFL.OTF",
                    FontSize = 14,
                    Text = string.Format(AppResources.Sexo) + ": " + test.user_sex,
                    TextColor = Colors.White
                }, 0, 2);

                grid.Add(new Label
                {
                    Padding = new Thickness(10, 0, 0, 0),
                    FontFamily = "SFL.OTF",
                    FontSize = 14,
                    Text = string.Format(AppResources.Edad) + ": " + test.user_age,
                    TextColor = Colors.White
                }, 0, 3);
                grid.Add(new Label
                {
                    Padding = new Thickness(10, 0, 0, 0),
                    FontFamily = "SFL.OTF",
                    FontSize = 14,
                    Text = string.Format(AppResources.Realizado) + ": " + test.date_done,
                    TextColor = Colors.White
                }, 0, 4);

                Border border = new Border

                {


                    Margin = new Thickness(0, 0, 0, 5),
                    BackgroundColor = Color.FromArgb("#00BF70"),
                    HeightRequest = 140,
                    StrokeShape = new RoundRectangle()
                    {
                        CornerRadius = new CornerRadius(15)
                    },
                    StrokeThickness = 0
            ,
                    Content = grid
                };


                tapGestureRecognizer.Tapped += (s, e) =>
                {
                    animarTestSeleccionado(s, e, test.user_id, test.user_sex, test.user_age, test.fact01, test.fact02, test.fact03, "s");
                };

                border.GestureRecognizers.Add(tapGestureRecognizer);

                border.TranslationY = 800;

                gridHistorial.Add(border, 0, contador);



                contador++;


            }

            var testsAlimentacion = await App.Database.GetFoodTestAsync();

            foreach (var test in testsAlimentacion)
            {

                TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();

                Grid grid = new Grid
                {
                    VerticalOptions = LayoutOptions.Start,
                    Padding = 5,
                    RowDefinitions =
        {
                new RowDefinition { Height = new GridLength(40) },
                new RowDefinition { Height = new GridLength(20) },
                new RowDefinition { Height = new GridLength(20) },
                new RowDefinition { Height = new GridLength(20) },
                new RowDefinition { Height = new GridLength(20) },
                new RowDefinition { Height = new GridLength(40) }


        }
                };


                grid.Add(new Rectangle
                {
                    Margin = new Thickness(230, 0, 0, 0),
                    BackgroundColor = Color.FromArgb("#ffb129"),
                    HeightRequest = 120,
                    Rotation = 20,
                    WidthRequest = 120
                }, 0, 5);

                grid.Add(new Rectangle
                {
                    Margin = new Thickness(40, 20, 0, 0),
                    BackgroundColor = Color.FromArgb("#ffb129"),
                    HeightRequest = 120,
                    Rotation = 150,
                    WidthRequest = 120
                }, 0, 5);

                grid.Add(new Label
                {
                    Padding = new Thickness(10, 10, 0, 0),
                    FontFamily = "SF.OTF",
                    FontSize = 14,
                    Text = string.Format(AppResources.TestTrastAlim),
                    TextColor = Colors.White
                }, 0, 0);

                grid.Add(new Label
                {
                    Padding = new Thickness(10, 0, 0, 0),
                    FontFamily = "SFL.OTF",
                    FontSize = 14,
                    Text = string.Format(AppResources.Nombre) + ": " + test.user_id,
                    TextColor = Colors.White
                }, 0, 1);

                grid.Add(new Label
                {
                    Padding = new Thickness(10, 0, 0, 0),
                    FontFamily = "SFL.OTF",
                    FontSize = 14,
                    Text = string.Format(AppResources.Sexo) + ": " + test.user_sex,
                    TextColor = Colors.White
                }, 0, 2);

                grid.Add(new Label
                {
                    Padding = new Thickness(10, 0, 0, 0),
                    FontFamily = "SFL.OTF",
                    FontSize = 14,
                    Text = string.Format(AppResources.Edad) + ": " + test.user_age,
                    TextColor = Colors.White
                }, 0, 3);
                grid.Add(new Label
                {
                    Padding = new Thickness(10, 0, 0, 0),
                    FontFamily = "SFL.OTF",
                    FontSize = 14,
                    Text = string.Format(AppResources.Realizado) + ": " + test.date_done,
                    TextColor = Colors.White
                }, 0, 4);

                Border border = new Border

                {


                    Margin = new Thickness(0, 0, 0, 5),
                    BackgroundColor = Colors.Orange,
                    HeightRequest = 140,
                    StrokeShape = new RoundRectangle()
                    {
                        CornerRadius = new CornerRadius(15)
                    },
                    StrokeThickness = 0
            ,
                    Content = grid
                };


                tapGestureRecognizer.Tapped += (s, e) =>
                {
                    animarTestSeleccionado(s, e, test.user_id, test.user_sex, test.user_age, test.total, 0, 0, "c");
                };

                border.GestureRecognizers.Add(tapGestureRecognizer);

                border.TranslationY = 800;

                gridHistorial.Add(border, 0, contador);


                contador++;


            }

            welcomeAnimacion();

        }

        public async void animarTestSeleccionado(object sender, EventArgs e, String nombre, String sexo, int edad, int evi, int cog, int fis, String tipo)
        {

            if (sender is Border border)
            {
                border.InputTransparent = true;

                await border.ScaleTo(0.95, 100);
                await border.ScaleTo(1, 50);

                await Navigation.PushModalAsync(new ResultPage(nombre, sexo, edad, evi, cog, fis, tipo, "h"));

                border.InputTransparent = false;

            }

        }

        private async void accionCerrar(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void welcomeAnimacion()
        {

            foreach (Border borde in gridHistorial.Children)
            {

                await borde.TranslateTo(0, -5, 300);
                await borde.TranslateTo(0, 0, 100);

            }

        }












    }

}
