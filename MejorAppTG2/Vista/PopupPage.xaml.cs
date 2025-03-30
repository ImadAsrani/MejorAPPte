using CommunityToolkit.Maui.Views;

namespace MejorAppTG2.Vista;

public partial class PopupPage : Popup
{


    private String titulo, cuerpo;

    public PopupPage(String titulo, String cuerpo, bool botonCancelar, int altura)
	{
		InitializeComponent();

        this.titulo = titulo;
        this.cuerpo = cuerpo;

        lblCuerpo.Text = cuerpo;
        lblTitulo.Text = titulo;

        if (botonCancelar)
        {
            btnCancelar.IsVisible = true;
        } else
        {
            btnCancelar.IsVisible = false;   
        }

        miPop.HeightRequest = altura;

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