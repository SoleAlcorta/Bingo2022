using Bingo2022.Models;
using Bingo2022.Rules;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bingo2022.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        //Conección BD.
        private readonly IConfiguration _configuration;

        //Lista donde se van a guardar los cartones.
        private static List<CartonModel>? _cartones;
        //Otros campos útiles.
        private static List<int>? _bolillero;
        private static int _bolillaSorteada = 0;
        private static int _marcador = 0;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration; 
        }

        public IActionResult Index()
        {
            //Instancio la lista donde voy a guardar los cartones.
            //_cartones = new List<CartonModel>();

            //Inicializo el bolillero;
            //var rule = new BolilleroRule();
            //_bolillero = rule.GenerarBolillero();


            return View();
        }

        public IActionResult Jugar()
        {
            //Acceso a la BD.
            string cs = _configuration.GetConnectionString("BingoDatabase");

            //Lista donde se van a guardar los cartones.
            _cartones = new List<CartonModel>();

            //Instancio el bolillero
            var rule = new BolilleroRule(_configuration);
            _bolillero = rule.GenerarBolillero();
            _marcador = 0;

            //Instancio la regla para generar los cartones.
            var ruleCartones = new CartonesRule(_configuration);
            var generados = ruleCartones.ObtenerNumeros();

            //Ciclo mediante el cual se asigna un N° de carton (1, 2, 3 o 4) y N° que va a contener
            for (int i = 0; i < 4; i++)
            {
                _cartones.Add(new CartonModel(i + 1, generados[i]));
            }

            return View(_cartones);
        }

        public IActionResult Sortear()
        {
            //Acceso a la BD.
            var ruleBolillero = new BolilleroRule(_configuration);
            var ruleCartones = new CartonesRule(_configuration);
         
            //Trae un numero del bolillero.
            _bolillaSorteada = _bolillero[_marcador];
            _marcador++;

            //Envio la bolilla a la BD.
            var guardarBolilla = new HistorialBolillero();
            guardarBolilla.NumBolilla = _bolillaSorteada;
            ruleBolillero.GuardarHistorialBolillero(guardarBolilla);

            var guardarGanadores = new HistorialCartones();

            //Variables para el msj ganador.
            string mensaje = "";
            string btnDisabled = "";

            //Asignar color a la bolilla.
            var ruleColor = new ColoresRule();
            string colorBolilla = ruleColor.ColorAleatorio();

            //Compara el num de bolilla con los num en los cartones.
            for (int vuelta = 0; vuelta < 4; vuelta++)
            {
                for (int c = 0; c < 9; c++)
                {
                    for (int f = 0; f < 3; f++)
                    {
                        //Si aparece, cambia la propiedad "salio" del num.
                        if (_cartones[vuelta].Numeros[f, c].Numero == _bolillaSorteada)
                        {
                            _cartones[vuelta].Numeros[f, c].Salio = true;

                            //Y contabiliza los aciertos del cartón.
                            _cartones[vuelta].Aciertos = _cartones[vuelta].Aciertos + 1;

                        }
                    }

                }

                //Cuando llega a los 15 aciertos, cambia el valor de la propiedad "Ganador".
                if (_cartones[vuelta].Aciertos == 15)
                {
                    _cartones[vuelta].Ganador = true;

                    //Lo que va a mostrar en pantalla.
                    mensaje = $"<div class=\"text-center text-black\"><h2 class=\"bg-danger\">FELICITACIONES</h2><h3>El carton N°{_cartones[vuelta].NumeroDeCarton} ha ganado!</h3></div>";
                    //mensaje = $"<h2 class=\"bg-danger\">FELICITACIONES</h2><h3>El carton N°{_cartones[vuelta].NumeroDeCarton} ha ganado!</h3>";

                    //Variable para desactivar el boton. 
                    btnDisabled = "disabled";

                    //Envio a la BD. 
                    if (guardarGanadores.Carton1 == null)
                    {
                        guardarGanadores.Carton1 = _cartones[vuelta].NumeroDeCarton;
                    }
                    else if (guardarGanadores.Carton2 == null)
                    {
                        guardarGanadores.Carton2 = _cartones[vuelta].NumeroDeCarton;
                    }
                    else if (guardarGanadores.Carton3 == null)
                    {
                        guardarGanadores.Carton3 = _cartones[vuelta].NumeroDeCarton;
                    } else guardarGanadores.Carton4 = _cartones[vuelta].NumeroDeCarton;

                    //Envio los ganadores a la BD.
                    ruleCartones.GuardarHistorialCartones(guardarGanadores);
                }

            }
            

            //Manda la bolilla para mostrarla en la vista.
            ViewData["bolilla"] = _bolillaSorteada;
            ViewData["color"] = colorBolilla;

            //El msj a mostrar y deshabilitar el boton.
            ViewData["msjGanador"] = mensaje;
            ViewData["classDisabled"] = btnDisabled;

            //Vuelve a mandar el carton a la vista.
            return View("Jugar", _cartones);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}