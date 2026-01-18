namespace PredictorActivos.ViewModel
{
    /// <summary>
    /// Modelo de salida que encapsula el resultado final
    /// del proceso de predicción para su presentación en la interfaz.
    /// </summary>
    public class PredictionResultViewModel
    {
        /// <summary>
        /// Identifica el algoritmo o enfoque de predicción aplicado
        /// durante el cálculo del resultado.
        /// </summary>
        public string Modo { get; set; } = string.Empty;

        /// <summary>
        /// Describe el comportamiento esperado del activo
        /// según el análisis realizado.
        /// </summary>
        public string Tendencia { get; set; } = string.Empty;

        /// <summary>
        /// Información explicativa que complementa el resultado,
        /// incluyendo observaciones o conclusiones relevantes.
        /// </summary>
        public string Detalles { get; set; } = string.Empty;

        /// <summary>
        /// Estimación del valor futuro del activo obtenida
        /// a partir del modelo de predicción seleccionado.
        /// </summary>
        public decimal? ValorFuturo { get; set; }

        /// <summary>
        /// Secuencia de cálculos o pasos lógicos utilizados
        /// para generar el resultado presentado.
        /// </summary>
        public List<string> Calculos { get; set; } = new();

        /// <summary>
        /// Indica el estado general del proceso de predicción.
        /// true  → cálculo exitoso,
        /// false → error durante la ejecución.
        /// </summary>
        public bool IsSuccess { get; set; } = true;

        /// <summary>
        /// Detalle del error ocurrido cuando el proceso
        /// no pudo completarse correctamente.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
