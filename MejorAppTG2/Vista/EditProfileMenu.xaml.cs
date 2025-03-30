using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.PlatformConfiguration;
using MejorAppTG2.Localization;

namespace MejorAppTG2.Vista;

public partial class EditProfileMenu : ContentPage
{

    private String nombre, sexo, imagen;
    private int edad;
    private TaskCompletionSource<string> _taskCompletionSource;

    public EditProfileMenu()
    {
        InitializeComponent();

        nombre = Preferences.Get("nombre", string.Empty);
        sexo = Preferences.Get("sexo", string.Empty);
        edad = int.Parse(Preferences.Get("edad", string.Empty));
        imagen = Preferences.Get("imagen", string.Empty);

        cargarDatos();

    }


    // Metodo devolver algo al cerrar
    public Task<string> GetResultAsync()
    {
        _taskCompletionSource = new TaskCompletionSource<string>();
        return _taskCompletionSource.Task;
    }

    private async void btnCancelar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void btnConfirmar_Clicked(object sender, EventArgs e)
    {

        brdNombre.Stroke = Colors.LightGray;
        brdEdad.Stroke = Colors.LightGray;


        if (!string.IsNullOrWhiteSpace(entNombre.Text))
        {

            if (!string.IsNullOrWhiteSpace(entEdad.Text))
            {
                brdEdad.Stroke = Colors.LightGray;

                try
                {

                    edad = int.Parse(entEdad.Text);

                    if (edad > 11)
                    {

                        
                        Preferences.Set("nombre", entNombre.Text);
                        Preferences.Set("edad", entEdad.Text);
                        Preferences.Set("sexo", sexo);
                        Preferences.Set("imagen", imagen);


                        _taskCompletionSource.SetResult("modificar");
                        await Navigation.PopModalAsync();
                    }
                    else
                    {

                        lblError.Text = string.Format(AppResources.RegisterDebest);

                    }


                }
                catch (Exception ex)
                {

                    Console.WriteLine("No es un numero");

                }


            }
            else
            {

                brdEdad.Stroke = Colors.Red;
                lblError.Text = string.Format(AppResources.RegisterIngresaEdad);
                await brdEdad.ScaleTo(1.05, 200);
                await brdEdad.ScaleTo(1, 200);

            }



        } else
        {
            brdNombre.Stroke = Colors.Red;
            lblError.Text = string.Format(AppResources.aviso_nombre);
            await brdNombre.ScaleTo(1.05, 200);
            await brdNombre.ScaleTo(1, 200);

        }




    }

    private async void TapGestureRecognizer_Tapped()
    {

        var dlg = new PopUpSmall(imagen) { Parent = this };
        var result = await dlg.ShowAtAsync(this);

        imagen = dlg.getImagen;

        imgProfile.Source = imagen;


    }

    private async void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        await editPhoto.ScaleTo(0.8, 100);
        await editPhoto.ScaleTo(1, 100);

        TapGestureRecognizer_Tapped();

    }

    public void cargarDatos()
    {

        imgProfile.Source = imagen;

        entEdad.Text = edad+"";

        entNombre.Text = nombre;


        if (sexo == "masculino")
        {
            brdgenero1.Stroke = Color.FromArgb("#00bf70");

        } else if (sexo == "femenino")
        {
            brdgenero2.Stroke = Color.FromArgb("#00bf70");

        } else
        {
            brdgenero3.Stroke = Color.FromArgb("#00bf70");
        }


    }

    private async void funcionCambiarColorSexoSeleccionado(object sender, TappedEventArgs e)
    {

        brdgenero1.Stroke = Colors.LightGrey;
        brdgenero2.Stroke = Colors.LightGrey;
        brdgenero3.Stroke = Colors.LightGrey;

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



    }