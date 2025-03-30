using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MejorAppTG2.Data;
using Microsoft.Maui.Networking; // Para verificar si el dispositivo esta en la red

namespace MejorAppTG2.Services
{
    public class SyncService
    {
        private readonly MejorAppTDatabase localDb;
        private readonly FirebaseRealtimeService firebaseService;

        public SyncService(MejorAppTDatabase localDb, FirebaseRealtimeService firebaseService)
        {
            this.localDb = localDb;
            this.firebaseService = firebaseService;
        }

        // Metodo para sincronizar de ShorTest en SQLite con Firebase
        public async Task SyncDataAsync()
        {
            // Verificamos si el dispositivo se encuentra conectado a Internet
            if (!Connectivity.NetworkAccess.Equals(NetworkAccess.Internet)) {
                Console.WriteLine("No hay conexión a Internet. La sincronización no se puede realizar.");
                return;
            }

            try {
                // Recuperamos los datos no subidos
                var unsyncedShortTests = await localDb.GetShortTestAsync();
                var unsyncedTests = unsyncedShortTests.Where(test => !test.is_uploaded).ToList();

                foreach (var test in unsyncedTests) {
                    // Subimos cada registro a Firebase
                    await firebaseService.UploadShortTestAsync(test);//Llama al metodo de Fire

                    // Actualizamos en nuestro SQLite el estado como "subido"
                    test.is_uploaded = true;
                    await localDb.UpdateShortTestAsync(test);
                }

                Console.WriteLine("Sincronización completada.");
            } catch (Exception ex) {
                Console.WriteLine($"Error durante la sincronización: {ex.Message}");
            }
        }

        public async Task SyncDataAsyncLong()
        {
            // Verificamos si el dispositivo se encuentra conectado a Internet
            if (!Connectivity.NetworkAccess.Equals(NetworkAccess.Internet))
            {
                Console.WriteLine("No hay conexión a Internet. La sincronización no se puede realizar.");
                return;
            }

            try
            {
                // Recuperamos los datos no subidos
                var unsyncedShortTests = await localDb.GetLongTestAsync();
                var unsyncedTests = unsyncedShortTests.Where(test => !test.is_uploaded).ToList();

                foreach (var test in unsyncedTests)
                {
                    // Subimos cada registro a Firebase
                    await firebaseService.UploadLongTestAsync(test);//Llama al metodo de Fire

                    // Actualizamos en nuestro SQLite el estado como "subido"
                    test.is_uploaded = true;
                    await localDb.UpdateLongTestAsync(test);
                }

                Console.WriteLine("Sincronización completada.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error durante la sincronización: {ex.Message}");
            }
        }

        public async Task SyncDataAsyncFood()
        {
            // Verificamos si el dispositivo se encuentra conectado a Internet
            if (!Connectivity.NetworkAccess.Equals(NetworkAccess.Internet))
            {
                Console.WriteLine("No hay conexión a Internet. La sincronización no se puede realizar.");
                return;
            }

            try
            {
                // Recuperamos los datos no subidos
                var unsyncedFoodTests = await localDb.GetFoodTestAsync();
                var unsyncedTests = unsyncedFoodTests.Where(test => !test.is_uploaded).ToList();

                foreach (var test in unsyncedTests)
                {
                    // Subimos cada registro a Firebase
                    await firebaseService.UploadFoodTestAsync(test);//Llama al metodo de Fire

                    // Actualizamos en nuestro SQLite el estado como "subido"
                    test.is_uploaded = true;
                    await localDb.UpdateFoodTestAsync(test);
                }

                Console.WriteLine("Sincronización completada.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error durante la sincronización: {ex.Message}");
            }
        }
    }
}