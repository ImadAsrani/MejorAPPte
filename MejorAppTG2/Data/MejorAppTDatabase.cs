using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using MejorAppTG2.Modelo;
using static System.Net.Mime.MediaTypeNames;

namespace MejorAppTG2.Data
{
    public class MejorAppTDatabase
    {
        // Inicializar SQLite
        private readonly SQLiteAsyncConnection _database;
        public MejorAppTDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
        }

        // Inicializamos la base de datos y creamos las tablas
        public async Task InitializeAsync()
        {
            Console.WriteLine("Creando tablas en la base de datos...");
            try
            {
                await _database.CreateTableAsync<ShortTest>();
                Console.WriteLine("Tabla Test1 creada");
                await _database.CreateTableAsync<LongTest>();
                Console.WriteLine("Tabla Test2 creada");
                await _database.CreateTableAsync<FoodTest>();
                Console.WriteLine("Tabla Test3 creada");
                Console.WriteLine("**************LISTADO DE BBDD **************");
                LoadData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear las tablas: {ex.Message}");
            }
        }

        public async Task<bool> TableExists<T>() where T : new()
        {
            var tableInfo = await _database.GetTableInfoAsync(typeof(T).Name);
            return tableInfo.Any();
        }

        public Task<List<ShortTest>> GetShortTestAsync()
        {
            // Ordena por fechas descendente y limitamos por 3 test
            return _database.Table<ShortTest>()
                            .OrderByDescending(test => test.date_done)
                            .ToListAsync();
        }

        public Task<List<LongTest>> GetLongTestAsync()
        {
            // Ordena por fechas descendente y limitamos por 3 test
            return _database.Table<LongTest>()
                            .OrderByDescending(test => test.date_done)
                            .ToListAsync();
        }

        public Task<List<FoodTest>> GetFoodTestAsync()
        {
            // Ordena por fechas descendente y limitamos por 3 test
            return _database.Table<FoodTest>()
                            .OrderByDescending(test => test.date_done)
                            .ToListAsync();
        }

        public async void LoadData()
        {
            try
            {
                // Recuperar los datos de la base de datos
                var tests = await App.Database.GetShortTestAsync();

                // Mostrar datos en consola
                Console.WriteLine("=== SHORT TEST ===");
                foreach (var test in tests)
                {
                    Console.WriteLine($"Test ID: {test.id}, Id Usuario: {test.user_id},Sexo {test.user_sex}, Edad {test.user_age}, Fac01: {test.fact01},  Fac02: {test.fact02}, FAC03: {test.fact03}, Fecha: {test.date_done}, Subido a Firebase: {test.is_uploaded}");
                }

                // Recuperar los datos de la base de datos
                var tests2 = await App.Database.GetLongTestAsync();

                // Mostrar datos en consola
                Console.WriteLine("=== LONG TEST ===");
                foreach (var test in tests2)
                {
                    Console.WriteLine($"Test ID: {test.id}, Id Usuario: {test.user_id},Sexo {test.user_sex}, Edad {test.user_age}, Fac01: {test.fact01},  Fac02: {test.fact02}, FAC03: {test.fact03},  FAC04: {test.fact04}, Fecha: {test.date_done}, Subido a Firebase: {test.is_uploaded}");
                }

                var tests3 = await App.Database.GetFoodTestAsync();

                // Mostrar datos en consola
                Console.WriteLine("=== FOOD TEST ===");
                foreach (var test in tests3)
                {
                    Console.WriteLine($"Test ID: {test.id}, Id Usuario: {test.user_id},Sexo {test.user_sex}, Edad {test.user_age}, Total: {test.total}, Fecha: {test.date_done}, Subido a Firebase: {test.is_uploaded}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurriooooooooooo un error al cargar los datos: {ex.Message}");
            }
        }

        // Guardamos toda la informacion sobre el ShortTest
        public async Task SaveShortTestAsync(ShortTest test1)
        {
            await _database.InsertAsync(test1);
        }

        // Actualizamos toda la informacion sobre el Test1
        public async Task UpdateShortTestAsync(ShortTest test1)
        {
            // Guardar Test1
            await _database.UpdateAsync(test1);
        }

        public async Task UpdateLongTestAsync(LongTest test1)
        {
            // Guardar Test1
            await _database.UpdateAsync(test1);
        }

        // Guardamos toda la informacion sobre el Test2
        public async Task SaveTest2Async(LongTest test2)
        {
            Console.WriteLine($"Estoy dentro de SaveTest1Aync");
            // Guardar Test2
            await _database.InsertAsync(test2);
        }

        // Guardamos toda la informacion sobre el Test3
        public async Task SaveFoodAsync(FoodTest test3)
        {
            Console.WriteLine($"Estoy dentro de SaveTest1Aync");
            // Guardar Test3
            await _database.InsertAsync(test3);
        }
        public async Task UpdateFoodTestAsync(FoodTest test1)
        {
            // Guardar Test1
            await _database.UpdateAsync(test1);
        }
        public async Task DestroyBBDDAsync()
        {
            Console.WriteLine("Borrando BBDD...");
            string databasePath = _database.DatabasePath; // Cambia por la ruta real de tu BBDD

            try
            {
                // Cerrar la base de datos
                await _database.CloseAsync();

                if (File.Exists(databasePath))
                {
                    File.Delete(databasePath);
                    Console.WriteLine("BBDD borrada exitosamente.");
                }
                else
                {
                    Console.WriteLine("La BBDD no existe o ya fue borrada.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al borrar la BBDD: {ex.Message}");
            }
        }

        // Limpiar listados
        public async Task ClearAllTestsAsync()
        {
            await _database.DeleteAllAsync<ShortTest>();
            await _database.DeleteAllAsync<FoodTest>();
            await _database.DeleteAllAsync<LongTest>();
        }


    }
}