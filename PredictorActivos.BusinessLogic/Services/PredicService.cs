using System.Globalization;
using PredictorActivos.Models.DTO;
using PredictorActivos.Models.Enums;
using PredictorActivos.Models.Strategy;

namespace PredictorActivos.Models.Services
{
    /// <summary>
    /// Servicio principal de predicción del sistema.
    /// 
    /// Se encarga de:
    /// - Validar los datos de entrada
    /// - Seleccionar la estrategia adecuada según el modo activo
    /// - Ejecutar el cálculo de predicción
    /// 
    /// Implementa el patrón Strategy para permitir
    /// múltiples algoritmos de predicción intercambiables.
    /// </summary>
    public class PredicService : IPredicService
    {
        /// <summary>
        /// Diccionario que relaciona cada modo de predicción
        /// con su estrategia concreta de cálculo.
        /// </summary>
        private readonly Dictionary<PredictionModo, IPredicStrategy> _strategies;

        /// <summary>
        /// Constructor del servicio.
        /// 
        /// Registra y asocia todas las estrategias de predicción
        /// soportadas por el sistema.
        /// </summary>
        public PredicService()
        {
            _strategies = new Dictionary<PredictionModo, IPredicStrategy>
            {
                { PredictionModo.Sma, new SmaStrategy() },
                { PredictionModo.LinealRegrasion, new LinealRegressionStrategy() },
                { PredictionModo.Momentum, new MomentumStrategy() }
            };
        }

        /// <summary>
        /// Ejecuta el proceso de predicción utilizando la estrategia
        /// correspondiente al modo seleccionado.
        /// </summary>
        /// <param name="data">
        /// Datos del activo que incluyen:
        /// - Lista de precios históricos
        /// - Modo de predicción seleccionado
        /// </param>
        /// <returns>
        /// Resultado completo de la predicción.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Se lanza cuando los datos no cumplen las reglas mínimas de validación.
        /// </exception>
        public PredictionResultDto CalcularPrediction(ActivoDataDto data)
        {
            if (!ValidarDatos(data.Precios))
                throw new ArgumentException("Los datos proporcionados no cumplen con los criterios mínimos.");

            var strategy = _strategies[data.PredictionMode];
            return strategy.CalculoPrediction(data.Precios);
        }

        /// <summary>
        /// Convierte un texto en formato CSV a una lista estructurada
        /// de precios del activo.
        /// 
        /// Formato esperado por línea:
        /// YYYY-MM-DD, Valor
        /// </summary>
        /// <param name="cvsData">
        /// Texto con los datos históricos separados por líneas.
        /// </param>
        /// <returns>
        /// Lista de precios válidos extraídos del texto.
        /// </returns>
        public List<ActivosPrecio> FiltrarDatos(string cvsData)
        {
            var precios = new List<ActivosPrecio>();

            if (string.IsNullOrWhiteSpace(cvsData))
                return precios;

            var lines = cvsData.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var parts = line.Split(',');

                if (parts.Length != 2)
                    continue;

                if (DateTime.TryParse(parts[0], out DateTime fecha) &&
                    decimal.TryParse(
                        parts[1].Trim(),
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out decimal precio))
                {
                    precios.Add(new ActivosPrecio
                    {
                        Fecha = fecha,
                        Valor = precio
                    });
                }
            }

            return precios;
        }

        /// <summary>
        /// Valida que los datos de precios cumplan con los requisitos
        /// necesarios para ejecutar una predicción confiable.
        /// </summary>
        /// <param name="precios">
        /// Lista de precios históricos del activo.
        /// </param>
        /// <returns>
        /// <c>true</c> si:
        /// - Existen exactamente 20 registros
        /// - Todos los valores son mayores a cero
        /// 
        /// <c>false</c> en cualquier otro caso.
        /// </returns>
        public bool ValidarDatos(List<ActivosPrecio> precios)
        {
            return precios != null
                   && precios.Count == 20
                   && precios.All(p => p.Valor > 0);
        }
    }
}
