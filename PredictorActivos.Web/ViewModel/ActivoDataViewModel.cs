using System.ComponentModel.DataAnnotations;
using PredictorActivos.Models;

namespace PredictorActivos.ViewModel
{
    /// <summary>
    /// Modelo de presentación utilizado para capturar la información
    /// necesaria para realizar una predicción sobre un activo.
    /// Soporta carga masiva de datos o ingreso manual estructurado.
    /// </summary>
    public class ActivoDataViewModel
    {
        /// <summary>
        /// Texto que contiene la información histórica del activo
        /// cuando se utiliza el método de carga completa.
        /// </summary>
        [Required(ErrorMessage = "Debe proporcionar los datos del activo")]
        [StringLength(2000, ErrorMessage = "El contenido ingresado supera el límite permitido")]
        public string Data { get; set; } = string.Empty;

        /// <summary>
        /// Colección de registros individuales que representan
        /// el historial de precios del activo.
        /// Cada elemento corresponde a una fecha y su valor asociado.
        /// </summary>
        public List<ActivosPrecioInputModel> IndividualInput { get; set; } = new();

        /// <summary>
        /// Define el método de captura de datos:
        /// true  → ingreso manual de valores,
        /// false → carga de datos desde archivo o texto.
        /// </summary>
        public bool UsoIndividualInput { get; set; }

        /// <summary>
        /// Inicializa el modelo preparando un conjunto fijo
        /// de 20 registros para la entrada manual de datos.
        /// </summary>
        public ActivoDataViewModel()
        {
            for (int i = 0; i < 20; i++)
            {
                IndividualInput.Add(new ActivosPrecioInputModel
                {
                    Index = i + 1
                });
            }
        }
    }
}
