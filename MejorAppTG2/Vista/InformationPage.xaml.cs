using CommunityToolkit.Maui.Views;
using MejorAppTG2.Localization;
namespace MejorAppTG2.Vista;

public partial class InformationPage : Popup
{


    private String titulo, cuerpo;

    public InformationPage()
	{
		InitializeComponent();

        animacion();
  
	}


    private async void animacion()
    {
        do
        {

            await btnInfo.RotateTo(20, 600);
            await btnInfo.RotateTo(-20, 600);


        } while (true);

    }

    // Funcion borrar datos local
    private async void aceptarBorrarDatos(object sender, EventArgs e)
    {

        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        await CloseAsync(true, cts.Token);

    }

    // Funcion cerrar al dar click en cancelar
    private async void cerrarPopup(object sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        await CloseAsync(false, cts.Token);

    }
}