using PredictorActivos.Models.DTO;

namespace PredictorActivos.Models.Strategy
{
    /// <summary>
    /// Estrategia de predicción basada en Momentum (Rate of Change - ROC).
    /// 
    /// Analiza la velocidad de cambio del precio comparando el valor actual
    /// con el de un número fijo de períodos anteriores.
    /// </summary>
    public class MomentumStrategy : IPredicStrategy
    {
        /// <summary>
        /// Nombre descriptivo del modo de predicción utilizado.
        /// </summary>
        public string NombreModo => "Momentum (ROC)";

        /// <summary>
        /// Cantidad de períodos usados para calcular el Momentum.
        /// </summary>
        private const int PeriodosAnalisis = 5;

        /// <summary>
        /// Ejecuta el cálculo de Momentum sobre los precios históricos.
        /// 
        /// Fórmula utilizada:
        /// ROC = ((PrecioActual / PrecioPasado) - 1) × 100
        /// </summary>
        /// <param name="precios">
        /// Lista de precios históricos del activo.
        /// Debe contener exactamente 20 registros.
        /// </param>
        /// <returns>
        /// Resultado de la predicción con la tendencia detectada
        /// y el detalle de los cálculos realizados.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Se lanza cuando la lista no tiene exactamente 20 elementos.
        /// </exception>
        public PredictionResultDto CalculoPrediction(List<ActivosPrecio> precios)
        {
            if (precios.Count != 20)
                throw new ArgumentException("Se requieren exactamente 20 registros para calcular el Momentum.");

            // Ordenar precios por fecha (más reciente primero)
            var preciosOrdenados = precios
                .OrderByDescending(p => p.Fecha)
                .ToList();

            var calculos = new List<string>();

            // Cálculo del Momentum para cada período
            for (int i = 0; i < preciosOrdenados.Count; i++)
            {
                if (i < PeriodosAnalisis)
                {
                    calculos.Add(
                        $"Período {i + 1}: Precio = {preciosOrdenados[i].Valor:F2} → Momentum no disponible");
                }
                else
                {
                    var precioActual = preciosOrdenados[i].Valor;
                    var precioPasado = preciosOrdenados[i - PeriodosAnalisis].Valor;

                    var roc = ((precioActual / precioPasado) - 1) * 100;

                    calculos.Add(
                        $"Período {i + 1}: Precio actual = {precioActual:F2}, " +
                        $"Precio hace {PeriodosAnalisis} períodos = {precioPasado:F2}, " +
                        $"ROC = {roc:F2}%");
                }
            }

            // Evaluación del último período para determinar la tendencia
            var ultimoIndex = preciosOrdenados.Count - 1;
            var ultimoPrecio = preciosOrdenados[ultimoIndex].Valor;
            var precioComparado = preciosOrdenados[ultimoIndex - PeriodosAnalisis].Valor;

            var ultimoRoc = ((ultimoPrecio / precioComparado) - 1) * 100;
            var tendencia = ultimoRoc > 0 ? "Alcista" : "Bajista";

            return new PredictionResultDto
            {
                Modo = NombreModo,
                Tendencia = tendencia,
                Detalles = $"Momentum final (ROC): {ultimoRoc:F2}%",
                Calculos = calculos
            };
        }
    }
}
