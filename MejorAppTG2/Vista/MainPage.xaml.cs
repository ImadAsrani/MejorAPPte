using System.Collections.ObjectModel;
using MejorAppTG2.Vista;
using MejorAppTG2.Modelo;
using System.Timers;
using CommunityToolkit.Maui.Views;
using System.Windows;
using MejorAppTG2.Localization;
using static System.Net.Mime.MediaTypeNames;
using FoodTest = MejorAppTG2.Vista.FoodTest;
using ShortTest = MejorAppTG2.Vista.ShortTest;
using LongTest = MejorAppTG2.Vista.LongTest;
using MejorAppTG2.Data;

namespace MejorAppTG2
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<CarouselItem> CarouselItems { get; set; }
        private System.Timers.Timer _carouselTimer; // Especificamos el namespace completo
        private int _currentIndex = 0;

        // Variables 

        private string nombre, sexo, imagen;
        private int edad;
        private int porcentaje = 0;
        // Campo estatico para la base de datos
        static MejorAppTDatabase database;

        public static MejorAppTDatabase Database
        {
            get
            {
                if (database == null)
                {
                    Console.WriteLine("Inicializando la base de datos...");
                    string dbPath = System.IO.Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "DEVMejorAppT.db3");
                    database = new MejorAppTDatabase(dbPath);
                }
                return database;
            }

        }

        public MainPage()
        {
            InitializeComponent();

            // Configura el temporizador
            _carouselTimer = new System.Timers.Timer(7000); // 7000 milisegundos = 7 segundos
            _carouselTimer.Elapsed += OnTimerElapsed;
            _carouselTimer.AutoReset = true;
            _carouselTimer.Start();


        }

        // Método para inicializar la base de datos
        private async void InitializeDatabaseAsync()
        {
            await Database.InitializeAsync();
        }

        protected override void OnAppearing()
        {

            base.OnAppearing();
            checkUser();

        }


        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                // Si hay elementos en el CarouselView
                if (carouselView.ItemsSource is System.Collections.IList items && items.Count > 0)
                {
                    _currentIndex = (_currentIndex + 1) % items.Count; // Avanza al siguiente elemento
                    carouselView.Position = _currentIndex; // Cambia la posici n
                }
            });

        }






        // Metodo para abrir opciones de perfil al hacer tap.
        private async void openProfileOptions(object sender, TappedEventArgs e)
        {


            profileBorder.InputTransparent = true;
            profileBorder.StrokeThickness = 2;
            await profileBorder.ScaleTo(1.05, 200);
            profileBorder.StrokeThickness = 1;
            await profileBorder.ScaleTo(1.0, 200);

            if (Preferences.ContainsKey("nombre"))
            {
                var menuPage = new Menu();
                var resultTask = menuPage.GetResultAsync();
                await Navigation.PushModalAsync(menuPage);

                profileBorder.InputTransparent = false;

                string result = await resultTask;


                if (result == "eliminar")
                {
                    btnEmpezar.IsVisible = true;

                    Preferences.Clear();

                    //await ClearTestHistory();

                    await App.Database.DestroyBBDDAsync();

                    desactivarTest();

                }else
                {
                }


            }
            else
            {

                var popup = new PopupPage(AppResources.MainAvisotit, AppResources.MainAvisoDescr, false, 200);
                var result = await this.ShowPopupAsync(popup, CancellationToken.None);
                profileBorder.InputTransparent = false;

            }







        }

        private async Task ClearTestHistory()
        {
            await App.Database.ClearAllTestsAsync();
        }




        // Metodo para navegaci n a pantalla de registro y seleccion test
        private async void GoToRegister(object sender, EventArgs e)
        {
            var registerPage = new RegisterPage();
            var resultTask = registerPage.GetResultAsync();

            await Navigation.PushModalAsync(registerPage);

            string[] result = await resultTask;


            // Accediendo a las partes
            nombre = result[0];
            edad = int.Parse(result[1]);
            sexo = result[2];
            imagen = result[3];

            // Comprobar si la base de datos esta inicializada
            if (database == null)
            {
                // Arranca la bbdd
                InitializeDatabaseAsync();
            }


        }




        // Verificar si el nombre est  almacenado en las preference, activar los test en caso de que sí, y desactivar en caso de borrar usuario

        private void checkUser()
        {

            Console.WriteLine("ESTO ES " + Preferences.Get("porcentajeTestCorto", "0"));

            if (Preferences.ContainsKey("nombre"))
            {

                    if (Preferences.ContainsKey("porcentajeTestCorto"))
                    {

                    progresoTestCorto.Percent = int.Parse(Preferences.Get("porcentajeTestCorto", string.Empty));
                        progresoTestCortoNumero.Text = Preferences.Get("porcentajeTestCorto", string.Empty);
                    }
                    else
                    {
                        progresoTestCorto.Percent = 0;
                        progresoTestCortoNumero.Text = 0 + "";
                    }

                    if (Preferences.ContainsKey("porcentajeTestLargo"))
                    {
                        progresoTestLargo.Percent = int.Parse(Preferences.Get("porcentajeTestLargo", string.Empty));
                        progresoTestLargoNumero.Text = Preferences.Get("porcentajeTestLargo", string.Empty);
                    }
                    else
                    {
                        progresoTestLargo.Percent = 0;
                        progresoTestLargoNumero.Text = 0 + "";
                    }
                    if (Preferences.ContainsKey("porcentajeTestAlimentacion"))
                    {
                        progresoTestAlimentacion.Percent = int.Parse(Preferences.Get("porcentajeTestAlimentacion", string.Empty));
                        progresoTestAlimentacionNumero.Text = Preferences.Get("porcentajeTestAlimentacion", string.Empty);
                    }
                    else
                    {
                        progresoTestAlimentacion.Percent = 0;
                        progresoTestAlimentacionNumero.Text = 0 + "";
                    }

                 
             


                nombre = Preferences.Get("nombre", string.Empty);
                sexo = Preferences.Get("sexo", string.Empty);
                imagen = Preferences.Get("imagen", string.Empty);
                edad = int.Parse(Preferences.Get("edad", string.Empty));


                lblEstado.Text = "Hey, " + nombre + "!";
                imgProfile.Source = imagen;

                if (btnEmpezar.IsVisible)
                {
                    activarTest();
                }


            }
            else
            {

                lblEstado.Text = "Hey!";

                imgProfile.Source = "Assets/profile.png";

            }


        }



        public async void activarTest()
        {
            btnEmpezar.InputTransparent = true;

            await Task.Delay(100);
            await btnEmpezar.TranslateTo(0, -15, 500);
            await Task.Delay(100);
            await btnEmpezar.TranslateTo(0, 200, 300);

            btnEmpezar.IsVisible = false;

            RowDefinition firstRow = gridPrincipal.RowDefinitions[0];
            firstRow.Height = 480;
            RowDefinition secondRow = gridPrincipal.RowDefinitions[1];
            secondRow.Height = 230;



            await rectest1.TranslateTo(330, 0, 0);
            await rectest2.TranslateTo(330, 0, 0);
            await rectest3.TranslateTo(330, 0, 0);

            rectest1.IsVisible = true;
            rectest2.IsVisible = true;
            rectest3.IsVisible = true;


            rectest1.TranslateTo(0, 0, 300);


            await Task.Delay(400);


            rectest2.TranslateTo(0, 0, 300);

            await Task.Delay(400);


            rectest3.TranslateTo(0, 0, 300);

            await scvPrincipal.ScrollToAsync(rectest2, ScrollToPosition.Center, true);

            btnEmpezar.InputTransparent = false;

        }




        public async void desactivarTest()
        {

            await Task.Delay(200);

            await rectest3.TranslateTo(200, 0, 200);
            await rectest2.TranslateTo(400, 0, 200);
            await rectest1.TranslateTo(600, 0, 200);

            rectest1.IsVisible = false;
            rectest2.IsVisible = false;
            rectest3.IsVisible = false;

            await rectest1.TranslateTo(-100, 200, 0);
            await rectest2.TranslateTo(-100, 200, 0);
            await rectest3.TranslateTo(-100, 200, 0);


            await btnEmpezar.TranslateTo(0, -15, 500);
            await Task.Delay(100);
            await btnEmpezar.TranslateTo(0, 0, 300);

            RowDefinition firstRow = gridPrincipal.RowDefinitions[0];
            firstRow.Height = 480;
            RowDefinition secondRow = gridPrincipal.RowDefinitions[1];
            secondRow.Height = 150;



        }


        public async void animarTestSeleccionado(object sender, EventArgs e)
        {

            if (sender is Border border)
            {
                border.InputTransparent = true;

                await border.ScaleTo(0.98, 100);
                await border.ScaleTo(1, 50);

                if (border.StyleId == "rectest1")
                {


                    await Navigation.PushModalAsync(new FoodTest());
      

                }
                else if (border.StyleId == "rectest2")
                {

                    await Navigation.PushModalAsync(new ShortTest());

        
                }
                else
                {

                    await Navigation.PushModalAsync(new LongTest());


                }


                border.InputTransparent = false;

            }

        }

    }

}