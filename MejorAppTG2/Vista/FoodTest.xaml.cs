using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using MejorAppTG2.Data;
using MejorAppTG2.Modelo;
using static System.Net.Mime.MediaTypeNames;
using MejorAppTG2.Services;
using CommunityToolkit.Maui.Views;
using System.Reflection;
using MejorAppTG2.Localization;

namespace MejorAppTG2.Vista
{
    public partial class FoodTest : ContentPage
    {
        // Lista de preguntas cargadas desde el archivo JSON
        private readonly List<Question> questions;

        // Diccionario para almacenar las respuestas seleccionadas (clave: índice de la pregunta, valor: respuesta)
        private readonly Dictionary<int, int> selectedAnswers = new();

        // Número de preguntas que se muestran por página
        private const int QuestionsPerPage = 5;

        // Página actual del cuestionario
        private int currentPage = 0;

        private TaskCompletionSource<string[]> _taskCompletionSource;

        private int percentage;
        private double total=5.2;
        private float sumaPreguntas;
        private bool activacionAnimacion = true;

        public FoodTest()
        {
            InitializeComponent();

            // Cargar preguntas desde el archivo JSON
            switch (App.DeviceLanguage)
            {
                case "es-ES":
                    questions = LoadQuestionsFromJson("alimenticio.json");  // Archivo para español
                    break;
                case "fr-FR":
                    questions = LoadQuestionsFromJson("Alimentacion.fr.json"); // Archivo para francés
                    break;
                case "en-US":
                    questions = LoadQuestionsFromJson("Alimentacion.en.json"); // Archivo para inglés
                    break;
                default:
                    questions = LoadQuestionsFromJson("alimenticio.json"); // Archivo predeterminado
                    break;
            }

            // Recuperar el progreso guardado, si existe
            int savedPage = Preferences.Get("foodCurrentPage", 0);
            currentPage = savedPage;

            // Recuperar las respuestas seleccionadas guardadas
            foreach (var question in questions)
            {
                int savedAnswer = Preferences.Get($"food_answer_{question.id}", 0);
                selectedAnswers[question.id] = savedAnswer;
            }

            // Renderizar las preguntas de la primera página
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

            string savedPercentageStr = Preferences.Get("porcentajeTestAlimentacion", "0");

            int savedPercentage = int.Parse(savedPercentageStr);


            if (savedPercentage == 0)
            {
                var popup = new InformationPage();
                var result = await this.ShowPopupAsync(popup, CancellationToken.None);

                if((bool) result){

                    var popup2 = new WarningPage();
                    var result2 = await this.ShowPopupAsync(popup2, CancellationToken.None);

                }




            }


        }

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
                        Preferences.Set("foodCurrentPage", currentPage);
                        string savedPercentageStr = Preferences.Get("porcentajeTestAlimentacion", "0");
                        int savedPercentage = int.Parse(savedPercentageStr);
                        // Comparar y guardar solo si el nuevo porcentaje es mayor
                        if (savedPercentage == 100)
                        {
                            Preferences.Set("porcentajeTestAlimentacion", percentage.ToString());
                        }
                        else
                        {
                            if (percentage > savedPercentage)
                            {
                                Preferences.Set("porcentajeTestAlimentacion", percentage.ToString());
                            }
                        }


                        await Navigation.PopModalAsync();

                    }

                }


            });

            return true;
        }

        public Task<string[]> GetResultAsync()
        {
            _taskCompletionSource = new TaskCompletionSource<string[]>();
            return _taskCompletionSource.Task;
        }


        /// <summary>
        /// Carga las preguntas desde un archivo JSON.
        /// </summary>
        /// <param name="fileName">Nombre del archivo JSON.</param>
        /// <returns>Lista de preguntas deserializadas.</returns>
        private List<Question> LoadQuestionsFromJson(string fileName)
        {
            try
            {
                // Abrir archivo JSON desde el paquete de la aplicación
                using var stream = FileSystem.OpenAppPackageFileAsync(fileName).Result;
                using var reader = new StreamReader(stream);
                string jsonContent = reader.ReadToEnd();

                // Deserializar el contenido JSON a una lista de preguntas
                return JsonSerializer.Deserialize<List<Question>>(jsonContent) ?? new List<Question>();
            }
            catch (Exception ex)
            {
                // Mostrar alerta en caso de error al cargar el archivo
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

                // Mostrar pregunta
                var questionLabel = new Label
                {
                    Text = $"{question.id}. {question.question}",
                    FontSize = 16,
                    FontFamily="SF.OTF",
                    Padding = new Thickness(40, 0, 40, 0)

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
        /// Actualiza la visibilidad de los botones de navegación (anterior, siguiente, enviar).
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
            else if(currentPage==0)
            {
                prevButton.InputTransparent = true;
                prevButton.IsVisible=false;
                prevButton.InputTransparent = false;

                activacionAnimacion = true;

            }


            if (currentPage == 4)
            {
                await nextButton.ScaleTo(1, 200);
                nextButton.IsVisible = true;
                BtnSend.IsVisible = false;


            } else if (currentPage > 4)
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
                            Preferences.Set($"food_answer_{question.id}", (int)rb.Value); // Guardar respuesta en Preferences
                            break;
                        }
                    }
                    start++;
                }
            }
        }

        /// <summary>
        /// Verifica si todas las preguntas en la página actual tienen una respuesta seleccionada.
        /// </summary>
        /// <returns>True si todas las preguntas tienen respuesta, de lo contrario false.</returns>
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

        /// <summary>
        /// Calcula la puntuación total basada en las respuestas seleccionadas.
        /// </summary>
        /// <returns>Puntuación total.</returns>
        private int CalculateTotalPoints()
        {
            return selectedAnswers.Sum(entry =>
            {
                int questionId = entry.Key;
                int answerValue = entry.Value;
                // Esta expresion es util cuando necesitas encontrar una pregunta especifica
                // en la lista basándote en su ID, en lugar de usar un índice numerico
                var question = questions.FirstOrDefault(q => q.id == questionId);

                if (question.id == 25)
                {
                    // Logica especial para la pregunta 25
                    return answerValue switch
                    {
                        1 => 3,
                        2 => 2,
                        3 => 1,
                        4 or 5 => 0,
                        _ => 0
                    };
                }
                else
                {
                    // Logica general
                    return answerValue switch
                    {
                        1 or 2 => 0,
                        3 => 1,
                        4 => 2,
                        5 => 3,
                        _ => 0
                    };
                }
            });
        }


        /// <summary>
        /// Evento que maneja el clic en el botón 'Siguiente'.
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
            total += 5.2;
            UpdateProgress();


        }

        private void OnPrevButtonClicked(object sender, EventArgs e)
        {
            SaveCurrentPageAnswers(); // Guardar respuestas antes de cambiar de página
            currentPage--;
            RenderQuestions();        // Renderizar nuevas preguntas
            total -= 5.2;
            UpdateProgress();
        }




        /** 
         * **Función BtnSend_Clicked**
         * **Funcion BtnSend_Clicked**
         * - Envía los datos y muestra un mensaje de agradecimiento si todas las preguntas están respondidas
         */

        // Evento para manejar el clic en el botón 'Enviar'
        /// <summary>
        /// Evento que maneja el clic en el botón 'Enviar'.
        /// </summary>

        private async void BtnSend_Clicked(object sender, EventArgs e)
        {
            SaveCurrentPageAnswers();

            if (AreAllQuestionsAnswered())
                
            {
                Preferences.Set("porcentajeTestAlimentacion", "100");

                int totalPoints = CalculateTotalPoints();
                
                // Recuperamos los datos del usuario 
                string user_id = Preferences.Get("nombre", string.Empty);
                string sexo = Preferences.Get("sexo", string.Empty);
                string edadString = Preferences.Get("edad", string.Empty);
                int edad = int.Parse(edadString);

                // Borrar respuestas de preferencias y limpiar diccionario a traves de hilos
                var keysToRemove = selectedAnswers.Keys.Select(qid => $"food_answer_{qid}").ToList();
                await Task.Run(() =>
                {
                    Preferences.Remove("foodCurrentPage");
                    foreach (var key in keysToRemove)
                        Preferences.Remove(key);

                    selectedAnswers.Clear();
                });

                // Limpiar el diccionario de respuestas seleccionadas
                selectedAnswers.Clear();

                String[] resultadoConsejos = new String[]
                {
                    user_id, sexo, edadString, totalPoints+"", "0", "0", "c"

                };


                // Creamos el objeto Test3 con sus atributos
                Modelo.FoodTest test3 = new Modelo.FoodTest
                {
                    user_id = user_id,
                    user_age = edad,
                    user_sex = sexo,
                    total = totalPoints,
                    date_done = DateTime.Now,
                    is_uploaded = false
                };

                // Guardamos los datos dentro  de la tabla Test3 en SQLite
                try
                {
                    if (await App.Database.TableExists<Modelo.FoodTest>())
                    {
                        await App.Database.SaveFoodAsync(test3);

                        // Intentamos sincronizar datos si hay conexion a Internet
                        if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                        {
                            var syncService = new SyncService(App.Database, new FirebaseRealtimeService());
                            await syncService.SyncDataAsyncFood(); // Subir datos a Firebase
                        }
                        else
                        {
                            Console.WriteLine("Sin conexión a Internet. El dato se sincronizará más tarde.");
                        }
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {



                }

                // Enviamos datos al mainpage
                // _taskCompletionSource.SetResult(resultadoConsejos);
                //  await Navigation.PopModalAsync(animated: false);

                await Navigation.PushModalAsync(new ResultPage(resultadoConsejos[0], resultadoConsejos[1], int.Parse(resultadoConsejos[2]), int.Parse(resultadoConsejos[3]), int.Parse(resultadoConsejos[4]), int.Parse(resultadoConsejos[5]), resultadoConsejos[6], "nada"));

            }
            else
            {

                var popup = new PopupPage(AppResources.NoTeOlv, AppResources.TienesQueRes, false, 220);
                var result = await this.ShowPopupAsync(popup, CancellationToken.None);


            }
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

    /// <summary>
    /// Clase que representa una pregunta del cuestionario.
    /// </summary>
    public class Question
    {
        public int id { get; set; }
        public string question { get; set; }
    }



}
