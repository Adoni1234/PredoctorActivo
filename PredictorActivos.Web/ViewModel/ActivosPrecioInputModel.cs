using System.ComponentModel.DataAnnotations;

namespace PredictorActivos.ViewModel
{
    /// <summary>
    /// Modelo auxiliar utilizado para capturar un registro puntual
    /// del historial de precios de un activo dentro de la interfaz.
    /// </summary>
    public class ActivosPrecioInputModel
    {
        /// <summary>
        /// Identificador visual del registro dentro del formulario.
        /// No representa un valor de negocio, solo orden y referencia.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Fecha asociada al valor del activo.
        /// Puede permanecer vacía hasta ser ingresada por el usuario.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? Fecha { get; set; }

        /// <summary>
        /// Precio del activo correspondiente a la fecha indicada.
        /// Se valida para evitar valores nulos o no positivos.
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "El valor del activo debe ser mayor que cero")]
        public decimal? Valor { get; set; }
    }
}
