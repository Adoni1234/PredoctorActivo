using PredictorActivos.Models.Enums;

namespace PredictorActivos.Models.Services
{
    /// <summary>
    /// Contrato que define la gestión del modo de predicción del sistema.
    /// 
    /// Esta interfaz centraliza la lógica relacionada con:
    /// - Consultar el modo de predicción activo
    /// - Cambiar dinámicamente el modo seleccionado
    /// - Proveer los modos disponibles para la interfaz de usuario
    /// </summary>
    public interface IPredicModoService
    {
        /// <summary>
        /// Retorna el modo de predicción que se encuentra actualmente activo
        /// y que será utilizado para realizar los cálculos.
        /// </summary>
        /// <returns>
        /// Enumeración <see cref="PredictionModo"/> correspondiente al modo actual.
        /// </returns>
        PredictionModo ModoActual();

        /// <summary>
        /// Actualiza el modo de predicción del sistema.
        /// 
        /// Este cambio afecta directamente la lógica utilizada
        /// en los procesos de predicción posteriores.
        /// </summary>
        /// <param name="modo">
        /// Nuevo modo de predicción seleccionado por el usuario.
        /// </param>
        void SetModo(PredictionModo modo);

        /// <summary>
        /// Obtiene la colección de modos de predicción disponibles.
        /// 
        /// Cada elemento incluye:
        /// - El valor de la enumeración
        /// - Un nombre legible para ser mostrado en la interfaz (dropdown, select, etc.)
        /// </summary>
        /// <returns>
        /// Lista de tuplas con el modo de predicción y su nombre descriptivo.
        /// </returns>
        List<(PredictionModo modo, string? Nombre)> ModosDisponibles();
    }
}
