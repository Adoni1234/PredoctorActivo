using PredictorActivos.Models.DTO;

namespace PredictorActivos.Models.Strategy
{
    /// <summary>
    /// Estrategia de predicción basada en Medias Móviles Simples (SMA Crossover).
    /// 
    /// Compara una media móvil de corto plazo (5 períodos)
    /// contra una media móvil de largo plazo (20 períodos)
    /// para identificar la tendencia del activo.
    /// </summary>
    public class SmaStrategy : IPredicStrategy
    {
        /// <summary>
        /// Nombre descriptivo del modo de predicción mostrado al usuario.
        /// </summary>
        public string NombreModo => "SMA Crossover";

        private const int PeriodosCorto = 5;
        private const int PeriodosLargo = 20;

        /// <summary>
        /// Ejecuta el cálculo de SMA Crossover.
        /// 
        /// Regla:
        /// - SMA corto > SMA largo  → Tendencia Alcista
        /// - SMA corto < SMA largo  → Tendencia Bajista
        /// </summary>
        /// <param name="precios">
        /// Lista de precios históricos del activo.
        /// Debe contener exactamente 20 registros.
        /// </param>
        /// <returns>
        /// Resultado de la predicción con tendencia y detalles del cálculo.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Se lanza si la lista no tiene exactamente 20 registros.
        /// </exception>
        public PredictionResultDto CalculoPrediction(List<ActivosPrecio> precios)
        {
            if (precios.Count != PeriodosLargo)
                throw new ArgumentException("Se requieren exactamente 20 registros para calcular SMA.");

            // Ordenar precios por fecha (más reciente primero)
            var preciosOrdenados = precios
                .OrderByDescending(p => p.Fecha)
                .ToList();

            // Cálculo de medias móviles
            var smaCorto = preciosOrdenados
                .Take(PeriodosCorto)
                .Average(p => p.Valor);

            var smaLargo = preciosOrdenados
                .Take(PeriodosLargo)
                .Average(p => p.Valor);

            // Determinar tendencia
            var tendencia = smaCorto > smaLargo ? "Alcista" : "Bajista";

            return new PredictionResultDto
            {
                Modo = NombreModo,
                Tendencia = tendencia,
                Detalles =
                    $"Media corta (5): {smaCorto:F2} | Media larga (20): {smaLargo:F2}",

                Calculos = new List<string>
                {
                    $"SMA corto (5 períodos): {smaCorto:F2}",
                    $"SMA largo (20 períodos): {smaLargo:F2}",
                    $"Comparación: SMA corto {(smaCorto > smaLargo ? ">" : "<")} SMA largo",
                    $"Conclusión: Tendencia {tendencia}"
                }
            };
        }
    }
}
