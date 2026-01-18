using PredictorActivos.Models.DTO;

namespace PredictorActivos.Models.Strategy
{
    /// <summary>
    /// Estrategia de predicción basada en Regresión Lineal.
    /// 
    /// Calcula una recta de tendencia a partir de los últimos 20 precios
    /// del activo y estima el valor esperado para el siguiente período.
    /// </summary>
    public class LinealRegressionStrategy : IPredicStrategy
    {
        /// <summary>
        /// Nombre descriptivo del modo de predicción que será mostrado
        /// en la interfaz de usuario y en los resultados.
        /// </summary>
        public string NombreModo => "Regresión Lineal";

        /// <summary>
        /// Ejecuta el algoritmo de regresión lineal sobre el histórico
        /// de precios proporcionado.
        /// 
        /// El modelo se basa en la ecuación:
        /// y = m·x + b
        /// </summary>
        /// <param name="precios">
        /// Lista de precios históricos del activo.
        /// Debe contener exactamente 20 registros.
        /// </param>
        /// <returns>
        /// Resultado completo de la predicción, incluyendo:
        /// - Tendencia
        /// - Valor futuro estimado
        /// - Detalles del cálculo
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Se lanza cuando la cantidad de precios es distinta de 20.
        /// </exception>
        public PredictionResultDto CalculoPrediction(List<ActivosPrecio> precios)
        {
            if (precios.Count != 20)
                throw new ArgumentException("Se requieren exactamente 20 registros de precios para aplicar la regresión lineal.");

            // Ordena los precios por fecha (del más reciente al más antiguo)
            var precioOrden = precios.OrderByDescending(p => p.Fecha).ToList();

            // Asigna un índice secuencial a cada precio
            for (int i = 0; i < precioOrden.Count; i++)
            {
                precioOrden[i].Index = i + 1;
            }

            var n = precioOrden.Count;

            // Cálculos necesarios para la fórmula de regresión lineal
            var sumaX = precioOrden.Sum(p => p.Index);
            var sumaY = precioOrden.Sum(p => (double)p.Valor);
            var sumaXy = precioOrden.Sum(p => p.Index * (double)p.Valor);
            var sumaX2 = precioOrden.Sum(p => p.Index * p.Index);

            // Pendiente (m) e intersección (b)
            var m = (n * sumaXy - sumaX * sumaY) / (n * sumaX2 - sumaX * sumaX);
            var b = (sumaY - m * sumaX) / n;

            // Predicción del valor futuro para el siguiente período (índice 21)
            var valorFuturo = (decimal)(m * 21 + b);
            var valorActual = precioOrden.Last().Valor;

            // Determinación de la tendencia
            var tendencia = valorFuturo > valorActual ? "Alcista" : "Bajista";

            return new PredictionResultDto
            {
                Modo = NombreModo,
                Tendencia = tendencia,
                ValorFuturo = valorFuturo,
                Detalles = $"Valor actual: {valorActual:F2} | Valor estimado: {valorFuturo:F2}",
                Calculos = new List<string>
                {
                    $"Pendiente (m): {m:F4}",
                    $"Intersección (b): {b:F2}",
                    $"Valor actual: {valorActual:F2}",
                    $"Valor proyectado (período 21): {valorFuturo:F2}",
                    $"Tendencia resultante: {tendencia}"
                }
            };
        }
    }
}
