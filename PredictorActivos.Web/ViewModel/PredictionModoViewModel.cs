using Microsoft.AspNetCore.Mvc.Rendering;
using PredictorActivos.Models.Enums;

namespace PredictorActivos.ViewModel
{
    /// <summary>
    /// Modelo de presentación utilizado para administrar la
    /// configuración del modo de predicción desde la interfaz.
    /// Proporciona el estado actual y las opciones disponibles.
    /// </summary>
    public class PredictionModoViewModel
    {
        /// <summary>
        /// Modo de predicción activo seleccionado por el usuario.
        /// Se inicializa con un valor por defecto.
        /// </summary>
        public PredictionModo SelectedModo { get; set; } = PredictionModo.Sma;

        /// <summary>
        /// Conjunto de opciones que se muestran en la interfaz
        /// para permitir la selección del modo de predicción.
        /// </summary>
        public List<SelectListItem> ModosAvailable { get; set; } = new();

        /// <summary>
        /// Mensaje informativo que confirma la actualización
        /// del modo de predicción.
        /// </summary>
        public string SuccessMessage { get; set; } = string.Empty;
    }
}
