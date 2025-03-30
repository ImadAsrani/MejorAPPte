namespace MejorAppTG2.Vista;

using CommunityToolkit.Maui.Views;
using Material.Components.Maui.Extensions;
using System.ComponentModel;
using System.Threading.Tasks;
using MejorAppTG2.Localization;

public partial class Menu : ContentPage, INotifyPropertyChanged
{

    private String nombre, sexo, imagen;
    private int edad;
    private TaskCompletionSource<string> _taskCompletionSource;

    public Menu()
	{
		InitializeComponent();

        nombre = Preferences.Get("nombre", string.Empty);
        sexo = Preferences.Get("sexo", string.Empty);
        edad = int.Parse(Preferences.Get("edad", string.Empty));
        imagen = Preferences.Get("imagen", string.Empty);

        asignarDatos();
        

    }


    // Metodo devolver algo al cerrar
    public Task<string> GetResultAsync()
    {
        _taskCompletionSource = new TaskCompletionSource<string>();
        return _taskCompletionSource.Task;
    }


    // Asignar datos a la cabecera
    private void asignarDatos()
    {

        borderMenu.IsVisible = true;

        if (!string.IsNullOrEmpty(nombre))
        {


            lblNombre.Text = nombre; 
            lblDatos.Text = edad + " " +string.Format(AppResources.Anios)+", " + sexo;


        }
   

    }


    // Funcion borrar datos
    private async void borrarDatos()
    {
        brdMenu3.InputTransparent = true;


        var popup = new PopupPage(AppResources.EliminDatos, AppResources.SeEliminusu, true, 215);

        var result = await this.ShowPopupAsync(popup, CancellationToken.None);

        brdMenu3.InputTransparent = false;


        if (result is bool boolResult)
        {
            if (boolResult)
            {
                _taskCompletionSource.SetResult("eliminar");
                await Navigation.PopModalAsync(animated: false);

            }
            else
            {
            }
        }



    }


    // Funcion modificar datos
    private async void modificarDatos()
    {
        brdMenu1.InputTransparent = true;

        var editProfilePage = new EditProfileMenu();
        var resultTask = editProfilePage.GetResultAsync();
        await Navigation.PushModalAsync(editProfilePage);

        brdMenu1.InputTransparent = false;

        string result = await resultTask;


        if (result == "modificar")
        {

            lblNombre.Text =Preferences.Get("nombre", string.Empty);
            lblDatos.Text = Preferences.Get("edad", string.Empty) + string.Format(AppResources.Anios) + ", " + Preferences.Get("sexo", string.Empty); ;
            
            await Task.Delay(400);
            _taskCompletionSource.SetResult("modificar");
            await Navigation.PopModalAsync();


        }



    }

    // Funcion ver historial de test realizados
    private async void verHistorial()
    {


        var tests = await App.Database.GetShortTestAsync();
        var testlargos = await App.Database.GetLongTestAsync();
        var testalimentacion = await App.Database.GetFoodTestAsync();

        if (tests.Count() >0 || testlargos.Count()>0 || testalimentacion.Count() > 0)
        {
            brdMenu2.InputTransparent = true;

            //var history = new TestHistory() { Parent = this };
            //var result = await history.ShowAtAsync(this);


            await Navigation.PushModalAsync(new HistoryTest());


            brdMenu2.InputTransparent = false;
        }else
        {
            var popup = new PopupPage(AppResources.SinDatos, AppResources.NoharealNinT, false, 180);
            var result = await this.ShowPopupAsync(popup, CancellationToken.None);
        }
       

       
    }


    // Funcion cerrar ajustes
    private async void cerrarAjustes(object sender, EventArgs e)
    {

        await Navigation.PopModalAsync();

    }




    // Metodos de diseño

    private async void funcionSeleccion(object sender, TappedEventArgs e)
    {

        brdMenu1.Stroke = Color.FromArgb("#00000000");
        brdMenu2.Stroke = Color.FromArgb("#00000000");
        brdMenu3.Stroke = Color.FromArgb("#00000000");

        if (sender is Border tappedBorder)
        {

            // Do something with the Border, like changing its color:
            tappedBorder.Stroke = Color.FromArgb("#bfbfbf");

            string btnSeleccionado = tappedBorder.StyleId;

            if (btnSeleccionado == "brdMenu1")
            {
                modificarDatos();

            }
            else if (btnSeleccionado == "brdMenu2")
            {
                verHistorial();
            }
            else
            {
                borrarDatos(); ;
            }




        }

        await Task.Delay(200);

        brdMenu1.Stroke = Color.FromArgb("#00000000");
        brdMenu2.Stroke = Color.FromArgb("#00000000");
        brdMenu3.Stroke = Color.FromArgb("#00000000");


    }

}