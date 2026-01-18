using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PredictorActivos.Models.Services;
using PredictorActivos.ViewModel;

namespace PredictorActivos.Controllers
{
    /// <summary>
    /// Controlador responsable de administrar la configuración del
    /// motor de predicción, permitiendo seleccionar y persistir
    /// el modo de cálculo activo.
    /// </summary>
    public class PredictorModoController : Controller
    {
        private readonly IPredicModoService _modoService;

        /// <summary>
        /// Crea una instancia del controlador e inyecta el servicio
        /// que expone los modos de predicción disponibles y el modo activo.
        /// </summary>
        /// <param name="predicModoService">Servicio de configuración del motor de predicción.</param>
        public PredictorModoController(IPredicModoService predicModoService)
        {
            _modoService = predicModoService ?? throw new ArgumentNullException(nameof(predicModoService));
        }

        /// <summary>
        /// Presenta la pantalla de configuración donde el usuario
        /// puede consultar el modo actual y seleccionar uno diferente.
        /// </summary>
        /// <returns>Vista de configuración del modo de predicción.</returns>
        public IActionResult Index()
        {
            var modoActual = _modoService.ModoActual();
            var modos = _modoService.ModosDisponibles();

            var viewModel = new PredictionModoViewModel
            {
                SelectedModo = modoActual,
                ModosAvailable = modos.Select(m => new SelectListItem
                {
                    Value = ((int)m.modo).ToString(),
                    Text = m.Nombre,
                    Selected = m.modo == modoActual
                }).ToList()
            };

            return View(viewModel);
        }

        /// <summary>
        /// Procesa la selección enviada por el usuario y actualiza
        /// el modo de predicción utilizado por el sistema.
        /// </summary>
        /// <param name="model">Modelo con el modo seleccionado.</param>
        /// <returns>
        /// Vista de configuración con confirmación visual del cambio realizado.
        /// </returns>
        [HttpPost]
        public IActionResult Index(PredictionModoViewModel model)
        {
            if (ModelState.IsValid)
            {
                _modoService.SetModo(model.SelectedModo);
                model.SuccessMessage = "La configuración del modo fue actualizada correctamente.";
            }

            // Se vuelve a cargar la lista de modos para mantener la vista consistente
            var modos = _modoService.ModosDisponibles();
            model.ModosAvailable = modos.Select(m => new SelectListItem
            {
                Value = ((int)m.modo).ToString(),
                Text = m.Nombre,
                Selected = m.modo == model.SelectedModo
            }).ToList();

            return View(model);
        }
    }
}
