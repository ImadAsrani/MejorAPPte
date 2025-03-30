#if ANDROID
using Android.Content.Res;
using Android.Graphics; // Para Android.Graphics.Color
using MejorAppTG2.Data;
using System;
using System.IO;
using MejorAppTG2.Services;
using Microsoft.Maui.Networking; // Para Connectivity
#endif

using System.Globalization;

namespace MejorAppTG2
{
    public partial class App : Application
    {
        // Campo estatico para la base de datos
        static MejorAppTDatabase database;
        public static string DeviceLanguage { get; private set; }
        private readonly SyncService syncService;

        // Propiedad que inicializa y devuelve la instancia de la base de datos
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

        // Metodo de sincronizacion los datos si hay conexion a Internet
        private async Task SyncDataIfConnectedAsync()
        {
            // Verifica si hay conexión a Internet
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Console.WriteLine("Conexión a Internet detectada. Sincronizando datos...");
                await syncService.SyncDataAsync(); // Llama a SyncService
            }
            else
            {
                Console.WriteLine("No hay conexión a Internet. La sincronización se realizará cuando haya conexión.");
            }
        }

        // Método para inicializar la base de datos
        private async void InitializeDatabaseAsync()
        {
            await Database.InitializeAsync();
        }


        public App()
        {
            InitializeComponent();

            // Inicializar base de datos asincronamente
            InitializeDatabaseAsync();


            var culture = CultureInfo.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            // Obtener el idioma del dispositivo
            DeviceLanguage = CultureInfo.CurrentUICulture.Name;

            // Mostrar el idioma (por ejemplo, en una alerta)
            MainPage = new AppShell();
            Console.WriteLine("Idioma del dispositivo: " + DeviceLanguage);


            // Inicializa SyncService con las dependencias requeridas
            this.syncService = new SyncService(Database, new FirebaseRealtimeService());
            // Intentar sincronizar si hay conexión a Internet
            SyncDataIfConnectedAsync();
            MainPage = new AppShell();

#if ANDROID
            // Cambiar el color del cursor para controles Entry (solo en Android)
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
            {
                var androidColor = Android.Graphics.Color.ParseColor("#00bf70"); // Usamos Android.Graphics.Color aquí
                handler.PlatformView.TextCursorDrawable?.SetColorFilter(androidColor, PorterDuff.Mode.SrcIn);
                handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent); // Usamos Android.Graphics.Color aquí
            });

#endif

            // Desactivar modo oscuro
            Application.Current.UserAppTheme = AppTheme.Light;


        }
    }
}
