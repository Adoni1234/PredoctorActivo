using PredictorActivos.Models.Enums;

namespace PredictorActivos.Models.Services
{
    /// <summary>
    /// Servicio concreto encargado de gestionar el modo de predicción activo
    /// dentro del sistema.
    /// 
    /// Mantiene un único modo seleccionado y garantiza acceso seguro
    /// en escenarios concurrentes.
    /// </summary>
    public class PredicModoService : IPredicModoService
    {
        /// <summary>
        /// Modo de predicción actualmente seleccionado.
        /// Por defecto se inicializa en SMA.
        /// </summary>
        private PredictionModo _modoActual = PredictionModo.Sma;

        /// <summary>
        /// Objeto de sincronización para asegurar operaciones thread-safe
        /// al consultar o modificar el modo.
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// Obtiene el modo de predicción que se encuentra activo en el sistema.
        /// 
        /// El acceso está protegido para evitar inconsistencias
        /// en entornos multi-hilo.
        /// </summary>
        /// <returns>
        /// Enumeración correspondiente al modo de predicción actual.
        /// </returns>
        public PredictionModo ModoActual()
        {
            lock (_lock)
            {
                return _modoActual;
            }
        }

        /// <summary>
        /// Actualiza el modo de predicción que será utilizado
        /// en los cálculos posteriores.
        /// 
        /// Esta operación sobrescribe el modo previamente configurado.
        /// </summary>
        /// <param name="modo">
        /// Nuevo modo de predicción seleccionado por el usuario o el sistema.
        /// </param>
        public void SetModo(PredictionModo modo)
        {
            lock (_lock)
            {
                _modoActual = modo;
            }
        }

        /// <summary>
        /// Obtiene la lista completa de modos de predicción soportados
        /// por el sistema.
        /// 
        /// Esta información se utiliza principalmente para poblar
        /// controles de selección en la interfaz de usuario.
        /// </summary>
        /// <returns>
        /// Lista de tuplas que contienen:
        /// - El valor de la enumeración
        /// - El nombre legible del modo
        /// </returns>
        public List<(PredictionModo modo, string? Nombre)> ModosDisponibles()
        {
            return Enum.GetValues<PredictionModo>()
                .Select(m => (modo: m, Nombre: m.ToString()))
                .ToList();
        }
    }
}
