using Microsoft.AspNetCore.Mvc;
using PredictorActivos.Models;
using PredictorActivos.Models.DTO;
using PredictorActivos.Models.Services;
using PredictorActivos.ViewModel;

namespace PredictorActivos.Controllers
{
    /// <summary>
    /// Controlador principal del módulo de predicción.
    /// Gestiona la entrada de datos del activo, valida la información
    /// y coordina el proceso de cálculo de resultados.
    /// </summary>
    public class PredictorController : Controller
    {
        private readonly IPredicService _predicService;
        private readonly IPredicModoService _predicModoService;

        /// <summary>
        /// Inicializa el controlador e inyecta los servicios necesarios
        /// para la gestión del modo de predicción y el cálculo de resultados.
        /// </summary>
        /// <param name="predicModoService">Servicio encargado de definir el modo de predicción activo.</param>
        /// <param name="predicService">Servicio que procesa los datos y genera la predicción.</param>
        public PredictorController(
            IPredicModoService predicModoService,
            IPredicService predicService)
        {
            _predicService = predicService ?? throw new ArgumentNullException(nameof(predicService));
            _predicModoService = predicModoService ?? throw new ArgumentNullException(nameof(predicModoService));
        }

        /// <summary>
        /// Muestra la pantalla inicial donde el usuario ingresa
        /// la información histórica del activo.
        /// </summary>
        /// <returns>Vista principal con el modelo vacío.</returns>
        public IActionResult Index()
        {
            return View(new ActivoDataViewModel());
        }

        /// <summary>
        /// Recibe los datos enviados por el usuario, valida la información,
        /// prepara el modelo de predicción y ejecuta el cálculo.
        /// </summary>
        /// <param name="model">Datos ingresados desde la interfaz.</param>
        /// <returns>
        /// Vista de resultados si el proceso es exitoso,
        /// o la vista inicial con mensajes de error en caso contrario.
        /// </returns>
        [HttpPost]
        public IActionResult Index(ActivoDataViewModel model)
        {
            // Validación básica del modelo recibido
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                List<ActivosPrecio> preciosProcesados;

                // Cuando el usuario ingresa datos manualmente,
                // se exige un conjunto exacto de 20 registros válidos
                if (model.UsoIndividualInput)
                {
                    var entradasValidas = model.IndividualInput
                        .Where(x => x.Fecha.HasValue && x.Valor.HasValue)
                        .OrderBy(x => x.Fecha)
                        .ToList();

                    if (entradasValidas.Count != 20)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Debe ingresar exactamente 20 registros con fecha y valor.");
                        return View(model);
                    }

                    preciosProcesados = entradasValidas.Select(x => new ActivosPrecio
                    {
                        Fecha = x.Fecha!.Value,
                        Valor = x.Valor!.Value
                    }).ToList();
                }
                else
                {
                    // Procesamiento automático de los datos cargados
                    preciosProcesados = _predicService.FiltrarDatos(model.Data);
                }

                // Validación final de consistencia de datos
                if (!_predicService.ValidarDatos(preciosProcesados))
                {
                    ModelState.AddModelError(string.Empty,
                        "Los datos no cumplen con los criterios requeridos para la predicción.");
                    return View(model);
                }

                // Obtención del modo de predicción actualmente configurado
                var modoActual = _predicModoService.ModoActual();

                var datosPrediccion = new ActivoDataDto
                {
                    Precios = preciosProcesados,
                    PredictionMode = modoActual
                };

                // Ejecución del cálculo de predicción
                var resultado = _predicService.CalcularPrediction(datosPrediccion);

                if (resultado == null)
                {
                    throw new InvalidOperationException("No se obtuvo un resultado válido del motor de predicción.");
                }

                // Preparación del modelo de salida para la vista de resultados
                var viewModelResultado = new PredictionResultViewModel
                {
                    Modo = resultado.Modo ?? "No definido",
                    Tendencia = resultado.Tendencia ?? "Indeterminada",
                    Detalles = resultado.Detalles ?? "No hay información adicional",
                    ValorFuturo = resultado.ValorFuturo,
                    Calculos = resultado.Calculos ?? new List<string>(),
                    IsSuccess = true
                };

                return View("Resultado", viewModelResultado);
            }
            catch (Exception ex)
            {
                // Manejo centralizado de errores
                var errorViewModel = new PredictionResultViewModel
                {
                    IsSuccess = false,
                    ErrorMessage = $"Error durante el proceso de predicción: {ex.Message}",
                    Modo = string.Empty,
                    Tendencia = string.Empty,
                    Detalles = string.Empty,
                    Calculos = new List<string>()
                };

                return View("Resultado", errorViewModel);
            }
        }
    }
}
