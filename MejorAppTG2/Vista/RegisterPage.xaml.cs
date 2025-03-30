using CommunityToolkit.Maui.Views;
using MejorAppTG2.Modelo;
using MejorAppTG2.Localization;

namespace MejorAppTG2.Vista;

public partial class RegisterPage : ContentPage
{

    private String nombre, sexo, imagen;
    private int edad;
    int pos = 0;

    private TaskCompletionSource<string[]> _taskCompletionSource;
    public RegisterPage()
    {
        InitializeComponent();

    }

    public Task<string[]> GetResultAsync()
    {
        _taskCompletionSource = new TaskCompletionSource<string[]>();
        return _taskCompletionSource.Task;
    }



    // Metodo guardar estado actual al salir
    protected override bool OnBackButtonPressed()
    {

        if (!string.IsNullOrWhiteSpace(entSuperior.Text) || pos>0)
        {

            Dispatcher.Dispatch(async () =>
            {
                var popup = new PopupPage(AppResources.AvisoSalir, AppResources.RgstPDescSalir, true, 215);
                var result = await this.ShowPopupAsync(popup, CancellationToken.None);

                if (result is bool boolResult)
                {
                    if (boolResult)
                    {

                        await Navigation.PopModalAsync();

                    }

                }


            });

        }
       

        else if (string.IsNullOrWhiteSpace(entSuperior.Text) && pos == 0)
        {
            Dispatcher.Dispatch(async () =>
            {
                await Navigation.PopModalAsync();
            });
        }

        return true;
    }

    private async void funcionalidadBotonFlecha(object sender, EventArgs e)
    {

        if (pos == 0)
        {

            if (!string.IsNullOrWhiteSpace(entSuperior.Text))
            {

            nombre = entSuperior.Text;
            entSuperior.Text = "";
            entSuperior.Keyboard = Keyboard.Numeric;
            entSuperior.MaxLength = 2;
            lblSuperior.Opacity = 0;
            lblSuperior.Text = string.Format(AppResources.RegsterPreguntaEdad);
            lblSuperior.FadeTo(1, 1000);
            brdSuperior.Opacity = 0;
            brdSuperior.FadeTo(1, 2000);


                brdSuperior.Stroke = Colors.LightGray;
                lblSuperior.TextColor = Color.FromArgb("#00bf70");

                pos++;

            } else
            {
                brdSuperior.Stroke = Colors.Red;
                lblSuperior.TextColor = Colors.Red;
                lblSuperior.Text = string.Format(AppResources.aviso_nombre);
                await brdSuperior.ScaleTo(1.05, 200);
                await brdSuperior.ScaleTo(1, 200);


            }

        }
        else if (pos == 1)
        {

            if (!string.IsNullOrWhiteSpace(entSuperior.Text))
            {

                try
                {

                        edad = int.Parse(entSuperior.Text);

                    if (edad > 11)
                    {
                        brdSuperior.IsVisible = false;
                        lblSuperior.Opacity = 0;
                        lblSuperior.Text = string.Format(AppResources.RegisterPreguntaedad);
                        lblSuperior.FadeTo(1, 1000);
                        hslGenero.IsVisible = true;
                        pos++;
                        brdSuperior.Stroke = Colors.LightGray;
                        lblSuperior.TextColor = Color.FromArgb("#00bf70");
                        
                    } else
                    {
                        brdSuperior.Stroke = Colors.Red;
                        lblSuperior.TextColor = Colors.Red;
                        lblSuperior.Text = string.Format(AppResources.RegisterDebest);
                        await brdSuperior.ScaleTo(1.05, 200);
                        await brdSuperior.ScaleTo(1, 200);
                    }
                        

                }
                catch (Exception ex)
                {

                    Console.WriteLine("No es un numero");

                }


            } else
            {
                brdSuperior.Stroke = Colors.Red;
                lblSuperior.TextColor = Colors.Red;
                lblSuperior.Text = string.Format(AppResources.RegisterIngresaEdad);
                await brdSuperior.ScaleTo(1.05, 200);
                await brdSuperior.ScaleTo(1, 200);

            }

        } else if (pos==2)
        {

            if (!string.IsNullOrWhiteSpace(sexo))
            {
                lblSuperior.Margin = 0;
                lblSuperior.FadeTo(0, 1000);
                hslGenero.IsVisible = false;
                brdSuperior.IsVisible = false;
                lblSuperior.Opacity = 0;
                lblSuperior.Text = string.Format(AppResources.RegisterSAvatar);
                lblSuperior.FadeTo(1, 1000);
                gridAvatar.IsVisible = true;
                pos++;
                brdSuperior.Stroke = Colors.LightGray;
                lblSuperior.TextColor = Color.FromArgb("#00bf70");


            } else
            {

                lblSuperior.TextColor = Colors.Red;
                lblSuperior.Text = string.Format(AppResources.RegisterSGenero);

            }


        }

        else
        {
            if(!string.IsNullOrWhiteSpace(imagen))
            {

                // Guardamos usuario
                saveUser(nombre, edad, sexo, imagen);

                String[] datosUsuario = new String[]
                {

                    nombre, edad+"", sexo, imagen

                };

                // Enviamos datos al mainpage
                _taskCompletionSource.SetResult(datosUsuario);

                // Cerramos ventana actual
                await Navigation.PopModalAsync();

                lblSuperior.Margin = new Thickness(0, 30, 0, 0);




            }
            else
            {
                lblSuperior.TextColor = Colors.Red;
                lblSuperior.Text = string.Format(AppResources.RegisterSelectAvatar);
            }



        }

        }

    private void saveUser(String nombre, int edad, String sexo, String imagen)
    {

        // Guardar el nombre en las preferencias

            Preferences.Set("nombre", nombre);
            Preferences.Set("edad", edad+"");
            Preferences.Set("sexo", sexo);
            Preferences.Set("imagen", imagen);


        // Actualizar el Label con el nombre guardado

        User usuario = new User(nombre, edad, sexo, imagen);


    }


    // Funciones auxiliares
    private async void funcionCambiarColorSexoSeleccionado(object sender, TappedEventArgs e)
    {

        brdgenero1.Stroke = Color.FromArgb("#002e1b");
        brdgenero2.Stroke = Color.FromArgb("#002e1b");
        brdgenero3.Stroke = Color.FromArgb("#002e1b");

        if (sender is Border tappedBorder)
        {

            // Do something with the Border, like changing its color:
            tappedBorder.Stroke = Color.FromArgb("#00bf70");

            await tappedBorder.ScaleTo(1.1, 100);
            await tappedBorder.ScaleTo(1, 100);

            string generoSeleccionado = tappedBorder.StyleId;

            if (generoSeleccionado == "brdgenero1")
            {
                if (App.DeviceLanguage == "es-ES")
                {
                    sexo = "masculino";
                }
                else if (App.DeviceLanguage == "en-US")
                {
                    sexo = "male";
                }
                else if (App.DeviceLanguage == "fr-FR")
                {
                    sexo = "masculin";
                }
                else
                {
                    sexo = "masculino"; // Valor por defecto si el idioma no está definido
                }
            }
            else if (generoSeleccionado == "brdgenero2")
            {
                if (App.DeviceLanguage == "es-ES")
                {
                    sexo = "femenino";
                }
                else if (App.DeviceLanguage == "en-US")
                {
                    sexo = "female";
                }
                else if (App.DeviceLanguage == "fr-FR")
                {
                    sexo = "féminin";
                }
                else
                {
                    sexo = "femenino"; // Valor por defecto si el idioma no está definido
                }
            }
            else
            {
                if (App.DeviceLanguage == "es-ES")
                {
                    sexo = "otro";
                }
                else if (App.DeviceLanguage == "en-US")
                {
                    sexo = "other";
                }
                else if (App.DeviceLanguage == "fr-FR")
                {
                    sexo = "autre";
                }
                else
                {
                    sexo = "otro"; // Valor por defecto si el idioma no está definido
                }

            }

        }
    }

    private async void funcionCambiarAvatarSeleccionado(object sender, TappedEventArgs e)
    {


        var hijosBorder = gridAvatar.Children.OfType<Border>();

        foreach (Border border in hijosBorder)
        {
            border.StrokeThickness = 1.5;
            border.Stroke = Colors.Black;
        }

        if (sender is Border tappedBorder)
        {

            // Do something with the Border, like changing its color:
            tappedBorder.StrokeThickness = 3;
            tappedBorder.Stroke =Color.FromArgb("#00BF70");


            await tappedBorder.ScaleTo(1.2, 100);
            await tappedBorder.ScaleTo(1, 100);

            string urlImagen = tappedBorder.StyleId;

            if (urlImagen.Contains("1"))
            {
                imagen ="Assets/profile1.png";
            }
            else if (urlImagen.Contains("2"))
            {
                imagen = "Assets/profile2.png";

            }
            else if (urlImagen.Contains("3"))
            {

                imagen = "Assets/profile3.png";


            }
            else if (urlImagen.Contains("4"))
            {
                imagen = "Assets/profile4.png";

            }
            else if (urlImagen.Contains("5"))
            {
                imagen = "Assets/profile5.png";


            }
            else
            {

                imagen = "Assets/profile6.png";

            }

        }


    }


}
