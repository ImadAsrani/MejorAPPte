using Microsoft.Maui.Controls; // Espacio de nombres para MAUI, necesario para crear la UI.
using System;
using System.Collections.Generic; // Espacio de nombres para listas y diccionarios.
using System.IO; // Para trabajar con archivos.
using System.Linq; // Métodos LINQ para consultas.
using System.Text.Json; // Para serialización/deserialización JSON.
using MejorAppTG2.Data;
using MejorAppTG2.Modelo;
using static System.Net.Mime.MediaTypeNames;
using MejorAppTG2.Services;
using CommunityToolkit.Maui.Views;
using MejorAppTG2.Localization;

namespace MejorAppTG2.Vista
{
    public partial class LongTest : ContentPage
    {
        // Lista de preguntas cargadas desde el archivo JSON.
        private readonly List<Question> questions;

        // Diccionario para almacenar las respuestas seleccionadas.
        // La clave es el índice de la pregunta y el valor es la respuesta seleccionada.
        private readonly Dictionary<int, int> selectedAnswers = new();

        // Constante para definir cuántas preguntas se mostrarán por página.
        private const int QuestionsPerPage = 5;

        // Página actual en el cuestionario.
        private int currentPage = 0;

        private double total = 5.5;

        private int percentage;


        // Variables para acumular los puntos de cada tipo de pregunta.
        private int totalFis = 0; // Fisiológico.
        private int totalEvi = 0; // Evitativo.
        private int totalCog = 0; // Cognitivo.
        private int totalSit = 0; // Situacional.

        private TaskCompletionSource<string[]> _taskCompletionSource;

        private bool activacionAnimacion = true;


        // Constructor de la clase.
        public LongTest()
        {
            InitializeComponent();

            // Cargar preguntas desde un archivo JSON.
            
            switch (App.DeviceLanguage)
            {
                case "es-ES":
                    questions = LoadQuestionsFromJson("test_largo.json");  // Archivo para español
                    break;
                case "fr-FR":
                    questions = LoadQuestionsFromJson("test_largo.fr.json"); // Archivo para francés
                    break;
                case "en-US":
                    questions = LoadQuestionsFromJson("test_largo.en.json"); // Archivo para inglés
                    break;
                default:
                    questions = LoadQuestionsFromJson("test_largo.json"); // Archivo predeterminado
                    break;
            }
            // Recuperar el progreso guardado, si existe
            int savedPage = Preferences.Get("longCurrentPage", 0);
            currentPage = savedPage;

            // Recuperar las respuestas seleccionadas guardadas
            foreach (var question in questions)
            {
                int savedAnswer = Preferences.Get($"long_answer_{question.id}", 0);
                selectedAnswers[question.id] = savedAnswer;
            }

            // Renderizar las preguntas correspondientes a la primera página.
            RenderQuestions();

            inicializar();

            animacion();

        }



        public async void inicializar()
        {

            if (currentPage > 0)
            {
                prevButton.IsVisible = true;
            }

            total = (total * currentPage);

            UpdateProgress();

            string savedPercentageStr = Preferences.Get("porcentajeTestLargo", "0");

            int savedPercentage = int.Parse(savedPercentageStr);


            if (savedPercentage == 0)
            {
                var popup = new InformationPage();
                var result = await this.ShowPopupAsync(popup, CancellationToken.None);
            }


        }
        public Task<string[]> GetResultAsync()
        {
            _taskCompletionSource = new TaskCompletionSource<string[]>();
            return _taskCompletionSource.Task;
        }



        // Metodo guardar estado actual al salir
        protected override bool OnBackButtonPressed()
        {
            Dispatcher.Dispatch(async () =>
            {
              var popup = new PopupPage(AppResources.AvisoSalir, AppResources.SeGuardaraAvance, true, 215);
                var result = await this.ShowPopupAsync(popup, CancellationToken.None);

                if (result is bool boolResult)
                {
                    if (boolResult)
                    {
                        // Guardamos las respuestas seleccionadas, la pagina actual y el porcentaje
                        SaveCurrentPageAnswers();
                        Preferences.Set("longCurrentPage", currentPage);
                        string savedPercentageStr = Preferences.Get("porcentajeTestLargo", "0");
                        int savedPercentage = int.Parse(savedPercentageStr);

                        // Comparar y guardar solo si el nuevo porcentaje es mayor
                        if (savedPercentage == 100)
                        {
                            Preferences.Set("porcentajeTestLargo", percentage.ToString());
                        }
                        else
                        {
                            if (percentage > savedPercentage)
                            {
                                Preferences.Set("porcentajeTestLargo", percentage.ToString());
                            }
                        }


                        await Navigation.PopModalAsync();

                    }

                }


            });

            return true;
        }

        /// <summary>
        /// Carga las preguntas desde un archivo JSON especificado.
        /// </summary>
        /// <param name="fileName">Nombre del archivo JSON que contiene las preguntas.</param>
        /// <returns>Una lista de objetos Question.</returns>
        private List<Question> LoadQuestionsFromJson(string fileName)
        {
            try
            {
                // Abrir el archivo en el paquete de la aplicación.
                using var stream = FileSystem.OpenAppPackageFileAsync(fileName).Result;
                using var reader = new StreamReader(stream);

                // Leer el contenido del archivo.
                string jsonContent = reader.ReadToEnd();

                // Deserializar el contenido en una lista de preguntas.
                return JsonSerializer.Deserialize<List<Question>>(jsonContent) ?? new List<Question>();
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje de error si no se puede cargar el archivo.
                DisplayAlert("Error", $"No se pudo cargar el archivo JSON: {ex.Message}", "OK");
                return new List<Question>();
            }
        }

        // Renderiza las preguntas y las opciones de respuesta en la pagina actual.
        private void RenderQuestions()
        {
            questionContainer.Children.Clear();
            int start = currentPage * QuestionsPerPage;

            Console.WriteLine($"Renderizando preguntas para la página {currentPage}, inicio en índice {start}");

            for (int i = 0; i < QuestionsPerPage; i++)
            {
                int questionIndex = start + i;
                if (questionIndex >= questions.Count) break;


                var question = questions[questionIndex];

                var questionLabel = new Label
                {
                    Text = $"{question.id}. {question.question}",
                    FontSize = 14.5,
                    FontFamily = "SF.OTF",
                    Padding = new Thickness(30, 0, 30, 0)

                };
                questionContainer.Children.Add(questionLabel);

                // Crear opciones de respuesta
                var radioButtons = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 5, 0, 5),
                    Spacing = 10
                };

                // Obtener respuesta seleccionada
                int selectedValue = 0;
                if (selectedAnswers.TryGetValue(question.id, out selectedValue))
                {
                    Console.WriteLine($"Pregunta {question.id}: Respuesta guardada {selectedValue}");
                }

                for (int j = 1; j <= 5; j++)
                {
                    var radioButton = new RadioButton
                    {
                        Content = j.ToString(),
                        Value = j,
                        IsChecked = (selectedValue == j), // Marcar si coincide con el valor almacenado
                        GroupName = $"Question{question.id}"
                    };

                    radioButton.CheckedChanged += (sender, args) =>
                    {
                        if (args.Value)
                        {
                            selectedAnswers[question.id] = j; // Actualizar al seleccionar
                            Console.WriteLine($"Pregunta {question.id} actualizada a {j}");
                        }
                    };

                    radioButtons.Children.Add(radioButton);
                }

                questionContainer.Children.Add(radioButtons);
            }

            UpdateButtons();
        }


        private void UpdateProgress()
        {
            double progress = (double)(currentPage + 1) / Math.Ceiling((double)questions.Count / QuestionsPerPage);
            
            percentage = (int)(progress * 100);

            circuloProgreso.SweepAngle = (-360 * (float)total) / questions.Count;


        }


        /// <summary>
        /// Actualiza la visibilidad de los botones de navegación según la página actual.
        /// </summary>
        private async void UpdateButtons()
        {

            prevButton.InputTransparent = true;
            BtnSend.InputTransparent = true;
            nextButton.InputTransparent = true;

            if (currentPage == 1 && activacionAnimacion)
            {

                prevButton.IsVisible = true;
                activacionAnimacion = false;


            }
            else if (currentPage == 0)
            {
                prevButton.InputTransparent = true;
                prevButton.IsVisible = false;
                prevButton.InputTransparent = false;

                activacionAnimacion = true;

            }


            if (currentPage == 8)
            {
                await nextButton.ScaleTo(1, 200);
                nextButton.IsVisible = true;
                BtnSend.IsVisible = false;


            }
            else if (currentPage > 8)
            {


                await nextButton.ScaleTo(0, 100);
                nextButton.IsVisible = false;

                await BtnSend.ScaleTo(0, 100);
                BtnSend.IsVisible = true;
                await BtnSend.ScaleTo(1, 200);

            }

            nextButton.InputTransparent = false;
            prevButton.InputTransparent = false;
            BtnSend.InputTransparent = false;


        }

        // Guardamos las respuestas seleccionadas en la pagina actual
        private void SaveCurrentPageAnswers()
        {
            int start = currentPage * QuestionsPerPage;

            foreach (var child in questionContainer.Children)
            {
                if (child is StackLayout radioButtonContainer)
                {
                    foreach (var radioButton in radioButtonContainer.Children)
                    {
                        if (radioButton is RadioButton rb && rb.IsChecked)
                        {
                            var question = questions[start];
                            selectedAnswers[question.id] = (int)rb.Value;
                            Preferences.Set($"long_answer_{question.id}", (int)rb.Value); // Guardar respuesta en Preferences
                            break;
                        }
                    }
                    start++;
                }
            }
        }

        /// <summary>
        /// Verifica si todas las preguntas en la página actual han sido respondidas.
        /// </summary>
        /// <returns>True si todas tienen respuesta, de lo contrario False.</returns>
        private bool AreAllQuestionsAnswered()
        {
            foreach (var child in questionContainer.Children)
            {
                if (child is StackLayout radioButtonContainer)
                {
                    bool anySelected = false;
                    foreach (var radioButton in radioButtonContainer.Children)
                    {
                        if (radioButton is RadioButton rb && rb.IsChecked)
                        {
                            anySelected = true;
                            break;
                        }
                    }

                    if (!anySelected)
                    {
                        return false; // Alguna pregunta no ha sido respondida.
                    }
                }
            }
            return true;
        }

        // Calcula los puntos totales para cada tipo de pregunta en base a las respuestas seleccionadas.
        private void CalculateTotalPoints()
        {
            totalFis = 0;
            totalEvi = 0;
            totalCog = 0;
            totalSit = 0;

            foreach (var answer in selectedAnswers)
            {
                var question = questions.FirstOrDefault(q => q.id == answer.Key);
                if (question != null)
                {
                    int points = answer.Value switch
                    {
                        1 or 2 => 0, // Baja intensidad.
                        3 => 1, // Intensidad moderada.
                        4 => 2, // Alta intensidad.
                        5 => 3, // Muy alta intensidad.
                        _ => 0
                    };

                    // Sumar puntos al tipo correspondiente.
                    switch (question.Tipo)
                    {
                        case "FIS":
                            totalFis += points;
                            break;
                        case "EVI":
                            totalEvi += points;
                            break;
                        case "COG":
                            totalCog += points;
                            break;
                        case "SIT":
                            totalSit += points;
                            break;
                    }
                }
            }
        }



        /// <summary>
        /// Maneja el evento del botón "Siguiente".
        /// </summary>
        private async void OnNextButtonClicked(object sender, EventArgs e)
        {
            // Verificar si todas las preguntas de la página actual están respondidas
            if (!AreAllQuestionsAnswered())
            {
                var popup = new PopupPage(AppResources.NoTeOlv, AppResources.TienesQueRes, false, 220);
                var result = await this.ShowPopupAsync(popup, CancellationToken.None);
                return; // Detener el avance si hay preguntas sin responder
            }

            SaveCurrentPageAnswers(); // Guardar respuestas antes de cambiar de página
            currentPage++;
            RenderQuestions();        // Renderizar nuevas preguntas
            total += 5.5;
            UpdateProgress();

        }

        private void OnPrevButtonClicked(object sender, EventArgs e)
        {
            SaveCurrentPageAnswers(); // Guardar respuestas antes de cambiar de página
            currentPage--;
            RenderQuestions();        // Renderizar nuevas preguntas
            total -= 5.5;
            UpdateProgress();

        }

        /// <summary>
        /// Maneja el evento del botón "Enviar".
        /// </summary>
        /// 


        private async void BtnSend_Clicked(object sender, EventArgs e)
        {
            SaveCurrentPageAnswers();
            if (AreAllQuestionsAnswered())
            {
               
                Preferences.Set("porcentajeTestLargo", "100");
                // Recuperamos los datos del usuario 
                string user_id = Preferences.Get("nombre", string.Empty);
                string sexo = Preferences.Get("sexo", string.Empty);
                string edadString = Preferences.Get("edad", string.Empty);
                int edad = int.Parse(edadString);

                // Calculamos los totales
                CalculateTotalPoints();



                String[] resultadoConsejos = new String[]
                  {
                        user_id, sexo, edad+"", totalEvi+"", totalCog+"", totalFis+"", "l"
                  };



                // Creamos el Test 2 con sus atributos
                Modelo.LongTest test2 = new Modelo.LongTest
                {
                    user_id = user_id,
                    user_age = edad,
                    user_sex = sexo,
                    fact01 = totalFis,
                    fact02 = totalEvi,
                    fact03 = totalCog,
                    fact04 = totalSit,
                    date_done = DateTime.Now,
                    is_uploaded = false
                };

                // Borrar respuestas de preferencias y limpiar diccionario a traves de hilos
                var keysToRemove = selectedAnswers.Keys.Select(qid => $"long_answer_{qid}").ToList();
                await Task.Run(() =>
                {
                    Preferences.Remove("longCurrentPage");
                    foreach (var key in keysToRemove)
                        Preferences.Remove(key);

                    selectedAnswers.Clear();
                });


                // Guardamos los datos en la tabla Test2 dentro de SQLite
                try
                {
                    if (await App.Database.TableExists<Modelo.LongTest>())
                    {
                        await App.Database.SaveTest2Async(test2);

                        // Intentamos sincronizar datos si hay conexion a Internet
                        if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                        {
                            var syncService = new SyncService(App.Database, new FirebaseRealtimeService());
                            await syncService.SyncDataAsyncLong(); // Subir datos a Firebase
                        }
                        else
                        {
                            Console.WriteLine("Sin conexión a Internet. El dato se sincronizará más tarde.");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "La tabla Test1 no existe.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Ocurrió un eeeeeerror al guardar los datos: {ex.Message}", "OK");
                }


                // Enviamos datos al mainpage
                //_taskCompletionSource.SetResult(resultadoConsejos);
                //await Navigation.PopModalAsync(animated: false);

                await Navigation.PushModalAsync(new ResultPage(resultadoConsejos[0], resultadoConsejos[1], int.Parse(resultadoConsejos[2]), int.Parse(resultadoConsejos[3]), int.Parse(resultadoConsejos[4]), int.Parse(resultadoConsejos[5]), resultadoConsejos[6], "nada"));


            }
            else
            {
                var popup = new PopupPage(AppResources.NoTeOlv, AppResources.TienesQueRes, false, 220);
                var result = await this.ShowPopupAsync(popup, CancellationToken.None);
            }




        }

        /// <summary>
        /// Clase que representa una pregunta del cuestionario.
        /// </summary>
        public class Question
        {
            public int id { get; set; } // Identificador único de la pregunta.
            public string question { get; set; } // Texto de la pregunta.
            public string Tipo { get; set; } // Tipo de la pregunta (FIS, EVI, COG, SIT).
        }

        private async void btnAyuda_Clicked(object sender, EventArgs e)
        {

            var popup = new InformationPage();
            var result = await this.ShowPopupAsync(popup, CancellationToken.None);

        }

        private async void animacion()
        {
            do
            {

                await btnAyuda.TranslateTo(0, 0, 200);
                await btnAyuda.TranslateTo(0, -5, 300);
                await btnAyuda.TranslateTo(0, 0, 200);
                await btnAyuda.TranslateTo(0, -5, 300);
                await Task.Delay(5000);


            } while (true);
        }

    }
}
