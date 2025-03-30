using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MejorAppTG2.Modelo;
using Firebase.Database.Query;

namespace MejorAppTG2.Services
{
    public class FirebaseRealtimeService
    {
        private readonly FirebaseClient _firebaseClient;

        public FirebaseRealtimeService()
        {
            // Iniciamos la conexion con Firebase
            _firebaseClient = new FirebaseClient("https://mejorapptg2-default-rtdb.firebaseio.com/");
        }

        public async Task UploadShortTestAsync(ShortTest test)
        {
            try
            {
                // Creamos un objeto con los campos requeridos
                var shortTest = new
                {
                    user_id = test.user_id,
                    user_sex = test.user_sex,
                    user_age = test.user_age,
                    fact01 = test.fact01,
                    fact02 = test.fact02,
                    fact03 = test.fact03,
                    date_done = test.date_done
                };

                await _firebaseClient
                    .Child("ShortTest")
                    .PostAsync(shortTest);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al subir a Firebase: {ex.Message}");
            }
        }


        public async Task UploadLongTestAsync(LongTest test)
        {
            try
            {
                // Creamos un objeto con los campos requeridos
                var longTest = new
                {
                    user_id = test.user_id,
                    user_sex = test.user_sex,
                    user_age = test.user_age,
                    fact01 = test.fact01,
                    fact02 = test.fact02,
                    fact03 = test.fact03,
                    fact04 = test.fact04,
                    date_done = test.date_done
                };

                await _firebaseClient
                    .Child("LongTest")
                    .PostAsync(longTest);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al subir a Firebase: {ex.Message}");
            }
        }




        public async Task UploadFoodTestAsync(FoodTest test)
        {
            try
            {
                // Creamos un objeto con los campos requeridos
                var FoodTest = new
                {
                    user_id = test.user_id,
                    user_sex = test.user_sex,
                    user_age = test.user_age,
                    total = test.total,
                    date_done = test.date_done
                };

                await _firebaseClient
                    .Child("FoodTest")
                    .PostAsync(FoodTest);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al subir a Firebase: {ex.Message}");
            }
        }
        // Metodos similares para subir LongTest y FoodTest si es necesario
    }
}