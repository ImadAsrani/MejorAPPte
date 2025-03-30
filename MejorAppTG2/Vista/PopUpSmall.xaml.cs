using Material.Components.Maui;

namespace MejorAppTG2.Vista;

public partial class PopUpSmall : Popup
{

    String imagen, imagenBackup;
	public PopUpSmall(String imagen)
	{
		InitializeComponent();
        this.imagenBackup = imagen;
        this.imagen = imagen;

    }

    public string getImagen { get => imagen; set => imagen = value; }

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
            tappedBorder.Stroke = Color.FromArgb("#00BF70");


            await tappedBorder.ScaleTo(1.2, 100);
            await tappedBorder.ScaleTo(1, 100);

            string urlImagen = tappedBorder.StyleId;

            if (urlImagen.Contains("1"))
            {
                imagen = "Assets/profile1.png";
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
            else if (urlImagen.Contains("6"))
            {

                imagen = "Assets/profile6.png";

            } 

        }


    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Close();

    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        Close();
    }
}