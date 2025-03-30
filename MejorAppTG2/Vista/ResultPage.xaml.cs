using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using MejorAppTG2.Localization;


namespace MejorAppTG2.Vista;

public partial class ResultPage : ContentPage
{

    private int numFoto = 1;
    private int todos;

    private int index;
    private int totalFisiologicas;
    private int totalEvitativas;
    private int totalCognitivas;
    private string tipoTest;
    private string sexoUsuario;
    private int totalAlimenticio;
    private int edadUsuario;
    private List<Tips> consejos; // Lista de todos los consejos cargados desde el JSON.
    private List<Tips> consejosEspecificos; // Consejos seleccionados seg�n los IDs.
    private string v1;
    private int v2;
    private int v3;
    private int v4;
    private string v5;
    private string pagina;

    public ResultPage(string nombre, string sexo, int edad, int evi, int cog, int fis, string tipo, string pagina)
    {
        InitializeComponent();
        tipoTest = tipo;
        edadUsuario = edad;
        
            if (sexo == "male")
            {
                sexo = "masculino";
            }
            else if (sexo == "masculin")
            {
            sexo = "masculino";
            }
            else
            {
                sexo = "masculino"; // Valor por defecto si el idioma no está definido
            }
        
           
            if (sexo == "female")
            {
            sexo = "femenino";
            }
            else if (sexo == "féminin")
            {
                sexo = "femenino";
            }
            else
            {
                sexo = "femenino"; // Valor por defecto si el idioma no está definido
            }
       
            if (sexo == "other")
            {
                sexo = "otro";
            }
            else if (sexo == "autre")
            {
            sexo = "otro";
            }
            else
            {
                sexo = "otro"; // Valor por defecto si el idioma no está definido
            }


        sexoUsuario = sexo;
        totalFisiologicas = fis;
        totalCognitivas = cog;
        totalEvitativas = evi;
        totalAlimenticio = evi;
        this.pagina = pagina;
        index = 0;
        // Cargar los consejos desde el JSON.
        consejos = LoadTipsFromJson();
        consejosEspecificos = FiltrarConsejos();
        ActualizarConsejoActual();

        if (tipo == "c")
        {
            btnSalir2.IsVisible = true;
            btnAnterior.IsVisible = false;
            btnConsejos.IsVisible = false;
            circuloProgreso.IsVisible = false;
            btnSalir.IsVisible = false;

        } 

    }

    public ResultPage(string v1, int v2, int v3, int v4, string v5)
    {
        this.v1 = v1;
        this.v2 = v2;
        this.v3 = v3;
        this.v4 = v4;
        this.v5 = v5;
    }

    protected override bool OnBackButtonPressed()
    {
        Dispatcher.Dispatch(async () =>
        {
            var popup = new PopupPage(AppResources.Recuerda, AppResources.PodrasAcc, false, 240);
            var result = await this.ShowPopupAsync(popup, CancellationToken.None);

            if (result is bool boolResult)
            {
                if (boolResult)
                {
                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                }
            }
        });

        return true;

    }



    private List<int> SeleccionarConsejos()

    {
        List<int> consejosSeleccionados = new List<int>();
        string mensajeAdicional = string.Empty;

        // (en mi cabeza es c Coginitivo a alto l test largo) y asi con todos en chica 15-16


        bool cal13m = totalCognitivas > 34.1;
        bool cml13m = totalCognitivas > 26 && totalCognitivas <= 34.1;
        bool cbl13m = totalCognitivas <= 26;

        bool eal13m = totalEvitativas > 0;
        bool ebl13m = totalEvitativas == 0;

        bool fal13m = totalFisiologicas > 25;
        bool fml13m = totalFisiologicas > 16 && totalFisiologicas <= 25;
        bool fbl13m = totalFisiologicas <= 16;

        // en caso de que la edad de la chica sea 15-16 

        bool cal15m = totalCognitivas > 36;
        bool cml15m = totalCognitivas > 28 && totalCognitivas <= 36;
        bool cbl15m = totalCognitivas <= 28;

        bool eal15m = totalEvitativas > 1;
        bool eml15m = totalEvitativas <= 1;
        bool ebl15m = totalEvitativas == 0;

        bool fal15m = totalFisiologicas > 27;
        bool fml15m = totalFisiologicas > 12 && totalFisiologicas <= 27;
        bool fbl15m = totalFisiologicas <= 22;

        // en caso de que la edad de la chica sea 17-18


        bool cal17m = totalCognitivas > 35;
        bool cml17m = totalCognitivas > 30 && totalCognitivas <= 35;
        bool cbl17m = totalCognitivas <= 30;

        bool eal17m = totalEvitativas > 1;
        bool eml17m = totalEvitativas <= 1;
        bool ebl17m = totalEvitativas == 0;

        bool fal17m = totalFisiologicas > 32;
        bool fml17m = totalFisiologicas > 23 && totalFisiologicas <= 32;
        bool fbl17m = totalFisiologicas <= 23;

        // ahora lo mismo pero con los hombres ahora en vez de m sera h, puede parecer confuso porque 
        // se obtiene el dato como como femenino y masculino pero yo lo hago con hombre y mujer

        // Chicos de 13

        bool cal13h = totalCognitivas > 28;
        bool cml13h = totalCognitivas > 19 && totalCognitivas <= 28;
        bool cbl13h = totalCognitivas <= 19;

        bool eal13h = totalEvitativas > 0;
        bool ebl13h = totalEvitativas == 0;

        bool fal13h = totalFisiologicas > 17;
        bool fml13h = totalFisiologicas > 11 && totalFisiologicas <= 17;
        bool fbl13h = totalFisiologicas <= 11;


        //chicos de 15

        bool cal15h = totalCognitivas > 26;
        bool cml15h = totalCognitivas > 19 && totalCognitivas <= 26;
        bool cbl15h = totalCognitivas <= 19;

        bool eal15h = totalEvitativas > 1;
        bool eml15h = totalEvitativas <= 1;
        bool ebl15h = totalEvitativas == 0;

        bool fal15h = totalFisiologicas > 18;
        bool fml15h = totalFisiologicas > 12 && totalFisiologicas <= 18;
        bool fbl15h = totalFisiologicas <= 12;


        // Chicos de 17


        bool cal17h = totalCognitivas > 26;
        bool cml17h = totalCognitivas > 21 && totalCognitivas <= 26;
        bool cbl17h = totalCognitivas <= 21;

        bool eal17h = totalEvitativas > 2;
        bool eml17h = totalEvitativas <= 2;
        bool ebl17h = totalEvitativas == 0;

        bool fal17h = totalFisiologicas > 18;
        bool fml17h = totalFisiologicas > 12 && totalFisiologicas <= 18;
        bool fbl17h = totalFisiologicas <= 12;


        // Ahora vamos a hacer para otros

        // otros de 13

        bool cal13o = totalCognitivas > 27;
        bool cml13o = totalCognitivas > 22 && totalCognitivas <= 27;
        bool cbl13o = totalCognitivas <= 22;

        bool eal13o = totalEvitativas > 0;
        bool ebl13o = totalEvitativas == 0;

        bool fal13o = totalFisiologicas > 13;
        bool fml13o = totalFisiologicas > 13 && totalFisiologicas <= 29;
        bool fbl13o = totalFisiologicas <= 29;


        // otros de 15

        bool cal15o = totalCognitivas > 31;
        bool cml15o = totalCognitivas > 23 && totalCognitivas <= 31;
        bool cbl15o = totalCognitivas <= 23;

        bool eal15o = totalEvitativas > 2;
        bool eml15o = totalEvitativas <= 2;
        bool ebl15o = totalEvitativas == 0;

        bool fal15o = totalFisiologicas > 32;
        bool fml15o = totalFisiologicas > 20 && totalFisiologicas <= 32;
        bool fbl15o = totalFisiologicas <= 20;


        // otros de 17

        bool cal17o = totalCognitivas > 30;
        bool cml17o = totalCognitivas > 21 && totalCognitivas <= 30;
        bool cbl17o = totalCognitivas <= 21;

        bool eal17o = totalEvitativas > 1;
        bool eml17o = totalEvitativas <= 1;
        bool ebl17o = totalEvitativas == 0;

        bool fal17o = totalFisiologicas > 30;
        bool fml17o = totalFisiologicas > 25 && totalFisiologicas <= 30;
        bool fbl17o = totalFisiologicas <= 25;


        switch (tipoTest)
        {

            case "s":
                // Calcular la puntuación total (solo para "s").

                int puntuacionTotal = totalCognitivas + totalFisiologicas + totalEvitativas;

                if (puntuacionTotal >= 32 && puntuacionTotal <= 49)
                {

                    mensajeAdicional = "Tus niveles de ansiedad estan dentro de la normalidad. Observa la sintomatología para evitar que la situación te sobrepase.";

                }
                else if (puntuacionTotal >= 50 && puntuacionTotal <= 80)
                {

                    mensajeAdicional = "Ansiedad demasiado alta, es importante aprender a gestionarla para prevenir problemas emocionales y de salud.";

                }

                // Comprobaciones específicas para tipoTest "s".
                if (totalCognitivas <= 14 && totalFisiologicas <= 14 && totalEvitativas <= 1)
                {
                    consejosSeleccionados.AddRange(new List<int> { 17, 18, 19 });
                }
                else if (totalCognitivas >= 15 && totalFisiologicas <= 14 && totalEvitativas <= 1)
                {
                    consejosSeleccionados.AddRange(new List<int> { 5, 6, 7, 8, 9, 17, 18, 19, 14, 22, 20, 21 });
                }
                else if (totalCognitivas <= 14 && totalFisiologicas >= 15 && totalEvitativas <= 1)
                {
                    consejosSeleccionados.AddRange(new List<int> { 10, 11, 12, 13, 14, 17, 18, 19, 20, 21 });
                }
                else if (totalCognitivas <= 14 && totalFisiologicas <= 14 && totalEvitativas >= 2)
                {
                    consejosSeleccionados.AddRange(new List<int> { 10, 7, 15, 23, 24, 14, 20, 21 });
                }
                else if (totalCognitivas >= 15 && totalFisiologicas >= 15 && totalEvitativas >= 2)
                {
                    consejosSeleccionados.AddRange(new List<int> { 10, 16, 12, 15, 14, 17, 18, 19, 20, 21 });
                }
                else if (totalCognitivas >= 15 && totalFisiologicas >= 15 && totalEvitativas <= 1)
                {
                    consejosSeleccionados.AddRange(new List<int> { 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21 });
                }
                else if (totalCognitivas >= 15 && totalFisiologicas <= 14 && totalEvitativas >= 2)
                {
                    consejosSeleccionados.AddRange(new List<int> { 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                }
                else if (totalCognitivas <= 14 && totalFisiologicas >= 15 && totalEvitativas >= 2)
                {
                    consejosSeleccionados.AddRange(new List<int> { 10, 12, 15, 14, 17, 18, 19, 20, 21 });
                }


                break;

            case "l":
                if (sexoUsuario.ToLower() == "femenino")
                {
                    if (edadUsuario >= 12 && edadUsuario <= 14)
                    {
                        // Edad 12-14 años Femenino

                        if (fbl13m && ebl13m && cbl13m)
                        {

                            // Caso1

                            consejosSeleccionados.AddRange(new List<int> { 1, 2, 17, 18, 19 });
                        }

                        else if (fbl13m && ebl13m && cml13m)
                        {

                            // Caso2 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 5, 6, 7, 8, 9, 17, 18, 19, 14, 22, 20, 21 });
                        }

                        else if (fbl13m && ebl13m && cal13m)
                        {

                            // Caso2 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 5, 6, 7, 8, 9, 17, 18, 19, 14, 22, 20, 21 });
                        }

                        else if (fbl13m && eal13m && cbl13m)
                        {

                            // Caso4 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15 });
                        }

                        else if (fbl13m && eal13m && cml13m)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fbl13m && eal13m && cal13m)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fml13m && ebl13m && cbl13m)
                        {

                            // Caso3 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 11, 12, 13, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml13m && ebl13m && cml13m)
                        {

                            // Caso6 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fml13m && ebl13m && cal13m)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fml13m && eal13m && cbl13m)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml13m && eal13m && cml13m)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml13m && eal13m && cal13m)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal13m && ebl13m && cbl13m)
                        {

                            // Caso3 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 11, 12, 13, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal13m && ebl13m && cml13m)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fal13m && ebl13m && cal13m)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }
                        else if (fal13m && eal13m && cbl13m)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal13m && eal13m && cml13m)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal13m && eal13m && cal13m)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                    }
                    else if (edadUsuario >= 15 && edadUsuario <= 16)
                    {
                        // Edad 15-16 años Femenino

                        if (fbl15m && ebl15m && cbl15m)
                        {

                            // Caso1

                            consejosSeleccionados.AddRange(new List<int> { 1, 2, 17, 18, 19 });
                        }

                        else if (fbl15m && ebl15m && cml15m)
                        {

                            // Caso2 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 5, 6, 7, 8, 9, 17, 18, 19, 14, 22, 20, 21 });
                        }

                        else if (fbl15m && ebl15m && cal15m)
                        {

                            // Caso2 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 5, 6, 7, 8, 9, 17, 18, 19, 14, 22, 20, 21 });
                        }

                        else if (fbl15m && eml15m && cbl15m)
                        {

                            // Caso4 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 7, 15 });
                        }

                        else if (fbl15m && eml15m && cml15m)
                        {

                            // Caso7 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fbl15m && eml15m && cal15m)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fbl15m && eal15m && cbl15m)
                        {

                            // Caso4 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15 });
                        }

                        else if (fbl15m && eal15m && cml15m)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fbl15m && eal15m && cal15m)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fml15m && ebl15m && cbl15m)
                        {

                            // Caso3 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 11, 12, 13, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15m && ebl15m && cml15m)
                        {

                            // Caso6 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15m && ebl15m && cal15m)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15m && eml15m && cbl15m)
                        {

                            // Caso8 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15m && eml15m && cml15m)
                        {

                            // Caso5 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15m && eml15m && cal15m)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15m && eal15m && cbl15m)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15m && eal15m && cml15m)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15m && eal15m && cal15m)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15m && ebl15m && cbl15m)
                        {

                            // Caso3 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 11, 12, 13, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15m && ebl15m && cml15m)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15m && ebl15m && cal15m)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15m && eml15m && cbl15m)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15m && eml15m && cml15m)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15m && eml15m && cal15m)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15m && eal15m && cbl15m)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15m && eal15m && cml15m)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15m && eal15m && cal15m)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                    }
                    else if (edadUsuario >= 17)
                    {
                        // Edad 17-18 años Femenino

                        if (fbl17m && ebl17m && cbl17m)
                        {

                            // Caso1

                            consejosSeleccionados.AddRange(new List<int> { 1, 2, 17, 18, 19 });
                        }

                        else if (fbl17m && ebl17m && cml17m)
                        {

                            // Caso2 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 5, 6, 7, 8, 9, 17, 18, 19, 14, 22, 20, 21 });
                        }

                        else if (fbl17m && ebl17m && cal17m)
                        {

                            // Caso2 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 5, 6, 7, 8, 9, 17, 18, 19, 14, 22, 20, 21 });
                        }

                        else if (fbl17m && eml17m && cbl17m)
                        {

                            // Caso4 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 7, 15 });
                        }

                        else if (fbl17m && eml17m && cml17m)
                        {

                            // Caso7 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fbl17m && eml17m && cal17m)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fbl17m && eal17m && cbl17m)
                        {

                            // Caso4 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15 });
                        }

                        else if (fbl17m && eal17m && cml17m)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fbl17m && eal17m && cal17m)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fml17m && ebl17m && cbl17m)
                        {

                            // Caso3 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 11, 12, 13, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17m && ebl17m && cml17m)
                        {

                            // Caso6 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17m && ebl17m && cal17m)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17m && eml17m && cbl17m)
                        {

                            // Caso8 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17m && eml17m && cml17m)
                        {

                            // Caso5 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17m && eml17m && cal17m)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17m && eal17m && cbl17m)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17m && eal17m && cml17m)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17m && eal17m && cal17m)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17m && ebl17m && cbl17m)
                        {

                            // Caso3 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 11, 12, 13, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17m && ebl17m && cml17m)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17m && ebl17m && cal17m)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17m && eml17m && cbl17m)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17m && eml17m && cml17m)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17m && eml17m && cal17m)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17m && eal17m && cbl17m)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17m && eal17m && cml17m)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17m && eal17m && cal17m)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                    }
                }
                else if (sexoUsuario.ToLower() == "masculino")
                {


                    if (edadUsuario >= 12 && edadUsuario <= 14)
                    {

                        // Hombres de 12 - 14 años


                        if (fbl13h && ebl13h && cbl13h)
                        {

                            // Caso1

                            consejosSeleccionados.AddRange(new List<int> { 1, 2, 17, 18, 19 });
                        }

                        else if (fbl13h && ebl13h && cml13h)
                        {

                            // Caso2 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 5, 6, 7, 8, 9, 17, 18, 19, 14, 22, 20, 21 });
                        }

                        else if (fbl13h && ebl13h && cal13h)
                        {

                            // Caso2 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 5, 6, 7, 8, 9, 17, 18, 19, 14, 22, 20, 21 });
                        }

                        else if (fbl13h && eal13h && cbl13h)
                        {

                            // Caso4 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15 });
                        }

                        else if (fbl13h && eal13h && cml13h)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fbl13h && eal13h && cal13h)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fml13h && ebl13h && cbl13h)
                        {

                            // Caso3 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 11, 12, 13, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml13h && ebl13h && cml13h)
                        {

                            // Caso6 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fml13h && ebl13h && cal13h)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fml13h && eal13h && cbl13h)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml13h && eal13h && cml13h)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml13h && eal13h && cal13h)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal13h && ebl13h && cbl13h)
                        {

                            // Caso3 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 11, 12, 13, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal13h && ebl13h && cml13h)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fal13h && ebl13h && cal13h)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }
                        else if (fal13h && eal13h && cbl13h)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal13h && eal13h && cml13h)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal13h && eal13h && cal13h)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                    }
                    else if (edadUsuario >= 15 && edadUsuario <= 16)
                    {

                        // Hombres de 15 - 16 años


                        if (fbl15h && ebl15h && cbl15h)
                        {

                            // Caso1

                            consejosSeleccionados.AddRange(new List<int> { 1, 2, 17, 18, 19 });
                        }

                        else if (fbl15h && ebl15h && cml15h)
                        {

                            // Caso2 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 5, 6, 7, 8, 9, 17, 18, 19, 14, 22, 20, 21 });
                        }

                        else if (fbl15h && ebl15h && cal15h)
                        {

                            // Caso2 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 5, 6, 7, 8, 9, 17, 18, 19, 14, 22, 20, 21 });
                        }

                        else if (fbl15h && eml15h && cbl15h)
                        {

                            // Caso4 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 7, 15 });
                        }

                        else if (fbl15h && eml15h && cml15h)
                        {

                            // Caso7 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fbl15h && eml15h && cal15h)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fbl15h && eal15h && cbl15h)
                        {

                            // Caso4 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15 });
                        }

                        else if (fbl15h && eal15h && cml15h)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fbl15h && eal15h && cal15h)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fml15h && ebl15h && cbl15h)
                        {

                            // Caso3 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 11, 12, 13, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15h && ebl15h && cml15h)
                        {

                            // Caso6 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15h && ebl15h && cal15h)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15h && eml15h && cbl15h)
                        {

                            // Caso8 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15h && eml15h && cml15h)
                        {

                            // Caso5 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15h && eml15h && cal15h)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15h && eal15h && cbl15h)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15h && eal15h && cml15h)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15h && eal15h && cal15h)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15h && ebl15h && cbl15h)
                        {

                            // Caso3 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 11, 12, 13, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15h && ebl15h && cml15h)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15h && ebl15h && cal15h)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15h && eml15h && cbl15h)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15h && eml15h && cml15h)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15h && eml15h && cal15h)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15h && eal15h && cbl15h)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15h && eal15h && cml15h)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15h && eal15h && cal15h)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                    }
                    else if (edadUsuario >= 17)
                    {

                        // Hombres de 17 - 18 años

                        if (fbl17h && ebl17h && cbl17h)
                        {

                            // Caso1

                            consejosSeleccionados.AddRange(new List<int> { 1, 2, 17, 18, 19 });
                        }

                        else if (fbl17h && ebl17h && cml17h)
                        {

                            // Caso2 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 5, 6, 7, 8, 9, 17, 18, 19, 14, 22, 20, 21 });
                        }

                        else if (fbl17h && ebl17h && cal17h)
                        {

                            // Caso2 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 5, 6, 7, 8, 9, 17, 18, 19, 14, 22, 20, 21 });
                        }

                        else if (fbl17h && eml17h && cbl17h)
                        {

                            // Caso4 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 7, 15 });
                        }

                        else if (fbl17h && eml17h && cml17h)
                        {

                            // Caso7 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fbl17h && eml17h && cal17h)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fbl17h && eal17h && cbl17h)
                        {

                            // Caso4 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15 });
                        }

                        else if (fbl17h && eal17h && cml17h)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fbl17h && eal17h && cal17h)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fml17h && ebl17h && cbl17h)
                        {

                            // Caso3 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 11, 12, 13, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17h && ebl17h && cml17h)
                        {

                            // Caso6 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17h && ebl17h && cal17h)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17h && eml17h && cbl17h)
                        {

                            // Caso8 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17h && eml17h && cml17h)
                        {

                            // Caso5 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17h && eml17h && cal17h)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17h && eal17h && cbl17h)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17h && eal17h && cml17h)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17h && eal17h && cal17h)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17h && ebl17h && cbl17h)
                        {

                            // Caso3 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 11, 12, 13, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17h && ebl17h && cml17h)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17h && ebl17h && cal17h)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17h && eml17h && cbl17h)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17h && eml17h && cml17h)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17h && eml17h && cal17h)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17h && eal17h && cbl17h)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17h && eal17h && cml17h)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17h && eal17h && cal17h)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                    }
                }
                else if (sexoUsuario.ToLower() == "otro")
                {
                    if (edadUsuario >= 12 && edadUsuario <= 14)
                    {

                        // Otros de 12 - 14 años

                        if (fbl13o && ebl13o && cbl13o)
                        {

                            // Caso1

                            consejosSeleccionados.AddRange(new List<int> { 1, 2, 17, 18, 19 });
                        }

                        else if (fbl13o && ebl13o && cml13o)
                        {

                            // Caso2 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 5, 6, 7, 8, 9, 17, 18, 19, 14, 22, 20, 21 });
                        }

                        else if (fbl13o && ebl13o && cal13o)
                        {

                            // Caso2 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 5, 6, 7, 8, 9, 17, 18, 19, 14, 22, 20, 21 });
                        }

                        else if (fbl13o && eal13o && cbl13o)
                        {

                            // Caso4 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15 });
                        }

                        else if (fbl13o && eal13o && cml13o)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fbl13o && eal13o && cal13o)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fml13o && ebl13o && cbl13o)
                        {

                            // Caso3 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 11, 12, 13, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml13o && ebl13o && cml13o)
                        {

                            // Caso6 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fml13o && ebl13o && cal13o)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fml13o && eal13o && cbl13o)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml13o && eal13o && cml13o)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml13o && eal13o && cal13o)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal13o && ebl13o && cbl13o)
                        {

                            // Caso3 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 11, 12, 13, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal13o && ebl13o && cml13o)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fal13o && ebl13o && cal13o)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }
                        else if (fal13o && eal13o && cbl13o)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal13o && eal13o && cml13o)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal13o && eal13o && cal13o)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                    }
                    if (edadUsuario >= 15 && edadUsuario <= 16)
                    {

                        // Otros de 15 - 16 años

                        if (fbl15o && ebl15o && cbl15o)
                        {

                            // Caso1

                            consejosSeleccionados.AddRange(new List<int> { 1, 2, 17, 18, 19 });
                        }

                        else if (fbl15o && ebl15o && cml15o)
                        {

                            // Caso2 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 5, 6, 7, 8, 9, 17, 18, 19, 14, 22, 20, 21 });
                        }

                        else if (fbl15o && ebl15o && cal15o)
                        {

                            // Caso2 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 5, 6, 7, 8, 9, 17, 18, 19, 14, 22, 20, 21 });
                        }

                        else if (fbl15o && eml15o && cbl15o)
                        {

                            // Caso4 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 7, 15 });
                        }

                        else if (fbl15o && eml15o && cml15o)
                        {

                            // Caso7 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fbl15o && eml15o && cal15o)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fbl15o && eal15o && cbl15o)
                        {

                            // Caso4 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15 });
                        }

                        else if (fbl15o && eal15o && cml15o)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fbl15o && eal15o && cal15o)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fml15o && ebl15o && cbl15o)
                        {

                            // Caso3 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 11, 12, 13, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15o && ebl15o && cml15o)
                        {

                            // Caso6 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15o && ebl15o && cal15o)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15o && eml15o && cbl15o)
                        {

                            // Caso8 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15o && eml15o && cml15o)
                        {

                            // Caso5 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15o && eml15o && cal15o)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15o && eal15o && cbl15o)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15o && eal15o && cml15o)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml15o && eal15o && cal15o)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15o && ebl15o && cbl15o)
                        {

                            // Caso3 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 11, 12, 13, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15o && ebl15o && cml15o)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15o && ebl15o && cal15o)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15o && eml15o && cbl15o)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15o && eml15o && cml15o)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15o && eml15o && cal15o)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15o && eal15o && cbl15o)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15o && eal15o && cml15o)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal15o && eal15o && cal15o)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                    }
                    if (edadUsuario >= 17)
                    {

                        // Otros de 17 - 18 años

                        if (fbl17o && ebl17o && cbl17o)
                        {

                            // Caso1

                            consejosSeleccionados.AddRange(new List<int> { 1, 2, 17, 18, 19 });
                        }

                        else if (fbl17o && ebl17o && cml17o)
                        {

                            // Caso2 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 5, 6, 7, 8, 9, 17, 18, 19, 14, 22, 20, 21 });
                        }

                        else if (fbl17o && ebl17o && cal17o)
                        {

                            // Caso2 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 5, 6, 7, 8, 9, 17, 18, 19, 14, 22, 20, 21 });
                        }

                        else if (fbl17o && eml17o && cbl17o)
                        {

                            // Caso4 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 7, 15 });
                        }

                        else if (fbl17o && eml17o && cml17o)
                        {

                            // Caso7 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fbl17o && eml17o && cal17o)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fbl17o && eal17o && cbl17o)
                        {

                            // Caso4 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15 });
                        }

                        else if (fbl17o && eal17o && cml17o)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fbl17o && eal17o && cal17o)
                        {

                            // Caso7 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 7, 15, 14, 9, 17, 18, 19, 20, 21 });
                        }

                        else if (fml17o && ebl17o && cbl17o)
                        {

                            // Caso3 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 11, 12, 13, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17o && ebl17o && cml17o)
                        {

                            // Caso6 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17o && ebl17o && cal17o)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17o && eml17o && cbl17o)
                        {

                            // Caso8 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17o && eml17o && cml17o)
                        {

                            // Caso5 + punto A

                            consejosSeleccionados.AddRange(new List<int> { 3, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17o && eml17o && cal17o)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17o && eal17o && cbl17o)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17o && eal17o && cml17o)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fml17o && eal17o && cal17o)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17o && ebl17o && cbl17o)
                        {

                            // Caso3 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 11, 12, 13, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17o && ebl17o && cml17o)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17o && ebl17o && cal17o)
                        {

                            // Caso6 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 6, 7, 12, 13, 17, 18, 19, 14, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17o && eml17o && cbl17o)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17o && eml17o && cml17o)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17o && eml17o && cal17o)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17o && eal17o && cbl17o)
                        {

                            // Caso8 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17o && eal17o && cml17o)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                        else if (fal17o && eal17o && cal17o)
                        {

                            // Caso5 + punto B

                            consejosSeleccionados.AddRange(new List<int> { 4, 10, 16, 12, 15, 14, 17, 18, 19, 20, 21, 29, 30, 31 });
                        }

                    }
                }
                break;


            case "c":
                if (totalAlimenticio >= 20)
                {

                    // Alto riesgo

                    consejosSeleccionados.AddRange(new List<int> { 28 });
                }

                else if (totalAlimenticio >= 15 && totalAlimenticio <= 19)
                {

                    // Riesgo moderado-alto

                    consejosSeleccionados.AddRange(new List<int> { 27 });
                }

                else if (totalAlimenticio >= 11 && totalAlimenticio <= 14)
                {

                    // Riesgo leve-moderado

                    consejosSeleccionados.AddRange(new List<int> { 26 });
                }

                else if (totalAlimenticio < 11)
                {

                    // Bajo riesgo

                    consejosSeleccionados.AddRange(new List<int> { 25 });
                }

                break;


            default:
                DisplayAlert("Error", $"El tipo de test '{tipoTest}' no es reconocido.", "OK");
                break;
        }

        return consejosSeleccionados;
    }
    private void ActualizarConsejoActual()
    {
        if (consejosEspecificos.Count > 0)
        {
            var consejoActual = consejosEspecificos[index % consejosEspecificos.Count];

            lblSuperior.Text = consejoActual.titulo;
            lblinferior.Text = consejoActual.descripcion;

            if (consejoActual.id == 9)
            {
                imgConsejos.IsVisible = false;
                videoWebView.IsVisible = true;
                carouselVideos.IsVisible = false;
                videoWebView.Source = "https://www.youtube.com/embed/jnOfzrDZYrA";
            } else if (consejoActual.id == 29) 
            {
                imgConsejos.IsVisible = false;
                videoWebView.IsVisible = true;
                carouselVideos.IsVisible = false;
                videoWebView.Source = "https://youtu.be/hV3A-BzfGZM";
            } else if (consejoActual.id == 30) 
            {
                imgConsejos.IsVisible = false;
                videoWebView.IsVisible = true;
                carouselVideos.IsVisible = false;
                videoWebView.Source = "https://youtu.be/C6bDxq1S7lw";
            } else if (consejoActual.id == 31) 
            {
                imgConsejos.IsVisible = false;
                videoWebView.IsVisible = true;
                carouselVideos.IsVisible = false;
                videoWebView.Source = "https://youtu.be/R5Ji-rDCCh0";
            }
            else if (consejoActual.id == 14)
                    {
                        imgConsejos.IsVisible = false;
                        videoWebView.IsVisible = false;
                        carouselVideos.IsVisible = true;

                        // Asegúrate de que el swipe esté habilitado al mostrar el consejo 10
                        carouselVideos.IsSwipeEnabled = true;
                    }
                    else
                    {
                        imgConsejos.IsVisible = true;
                        videoWebView.IsVisible = false;
                        carouselVideos.IsVisible = false;

                        imgConsejos.Source = "f" + numFoto + ".jpg";

                        numFoto = numFoto < 9 ? numFoto + 1 : 1;
                    }

            circuloProgreso.SweepAngle = -360 * (index + 1) / consejosEspecificos.Count;
        }
        else
        {
            lblSuperior.Text = "No se encontraron consejos.";
            lblinferior.Text = string.Empty;
        }
    }


    private void OnCarouselPositionChanged(object sender, PositionChangedEventArgs e)
    {
        var carousel = sender as CarouselView;

        // Activar el swipe siempre que no esté en los extremos
        if (carousel.Position == 0)
        {
            // Primer video: puede ir solo hacia la derecha
            carousel.IsSwipeEnabled = true;
        }
        else if (carousel.Position == 2) // Posición del último video
        {
            // Último video: puede ir solo hacia la izquierda
            carousel.IsSwipeEnabled = true;
        }
        else
        {
            // En cualquier otra posición, permitir el desplazamiento en ambos sentidos
            carousel.IsSwipeEnabled = true;
        }
    }



    private void OnVideoSelected(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is string videoUrl)
        {
            videoWebView.Source = videoUrl;
            videoWebView.IsVisible = true;
            carouselVideos.IsVisible = false;
        }
    }


    private void cambiarConsejos(object sender, EventArgs e)
    {
        if (consejosEspecificos.Count != todos+1)
        {

            index = (index + 1) % consejosEspecificos.Count;

            ActualizarConsejoActual();

            todos++;

        }

      
    }

    private List<Tips> FiltrarConsejos()
    {

        var consejosIds = SeleccionarConsejos();
        return consejos
            .Where(cons => consejosIds.Contains(cons.id))
            .ToList();

    }

    private List<Tips> LoadTipsFromJson()
    {
        try
        {

            // Nombre del archivo JSON que contiene las preguntas.

            string fileName;

            switch (App.DeviceLanguage)
            {
                case "es-ES":
                    fileName = "Consejos.json"; // Archivo para español
                    break;
                case "fr-FR":
                    fileName = "Consejos.fr.json"; // Archivo para francés
                    break;
                case "en-US":
                    fileName = "Consejos.en.json"; // Archivo para inglés
                    break;
                default:
                    fileName = "Consejos.json"; // Archivo predeterminado
                    break;
            }
            // Abrir el archivo dentro del paquete de la aplicaci�n.

            using var stream = FileSystem.OpenAppPackageFileAsync(fileName).Result;
            using var reader = new StreamReader(stream);


            // Leer el contenido del archivo JSON.

            string jsonContent = reader.ReadToEnd();


            // Deserializar el JSON en una lista de consejos.

            return JsonSerializer.Deserialize<List<Tips>>(jsonContent);

        }
        catch (Exception ex)
        {

            DisplayAlert("Error", $"No se pudo cargar el archivo JSON: {ex.Message}", "OK");
            return new List<Tips>();

        }
    }

    // Clase auxiliar Tips
    public class Tips
    {
        public int id { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
    }

    private async void btnAnterior_Clicked(object sender, EventArgs e)
    {

        if (index != 0)
        {

            index = (index - 1) % consejosEspecificos.Count;

            ActualizarConsejoActual();

            todos--;

        }

       
    }

    private async void btnSalir_Clicked(object sender, EventArgs e)
    {
        var popup = new PopupPage(AppResources.Recuerda, AppResources.PodrasAcc, false, 240);
        var result = await this.ShowPopupAsync(popup, CancellationToken.None);

        if (result is bool boolResult)
        {
            if (boolResult)
            {

                if (pagina == "h")
                {
                    await Navigation.PopModalAsync();
                }
                else
                {
                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                }

            }
        }
    }
}