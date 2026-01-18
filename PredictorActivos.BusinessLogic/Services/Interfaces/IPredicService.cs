using PredictorActivos.Models.DTO;

namespace PredictorActivos.Models.Services
{
    /// <summary>
    /// Contrato del servicio responsable de procesar los datos de los activos
    /// y ejecutar los algoritmos de predicción según el modo seleccionado.
    /// 
    /// Este servicio actúa como el núcleo lógico del sistema de predicción.
    /// </summary>
    public interface IPredicService
    {
        /// <summary>
        /// Ejecuta el proceso completo de predicción a partir de los datos suministrados.
        /// 
        /// El cálculo se realiza considerando:
        /// - Los precios históricos del activo
        /// - El modo de predicción actualmente configurado
        /// </summary>
        /// <param name="data">
        /// Objeto que contiene los datos necesarios del activo
        /// (precios, fechas y configuración de entrada).
        /// </param>
        /// <returns>
        /// Resultado estructurado de la predicción, incluyendo tendencia,
        /// valor estimado y detalles del cálculo.
        /// </returns>
        PredictionResultDto CalcularPrediction(ActivoDataDto data);

        /// <summary>
        /// Procesa y transforma un texto en formato CSV en una colección
        /// estructurada de precios del activo.
        /// 
        /// Cada línea debe respetar el formato:
        /// Fecha,Valor
        /// </summary>
        /// <param name="cvsData">
        /// Texto plano con los datos de precios separados por comas.
        /// </param>
        /// <returns>
        /// Lista de precios del activo con fecha y valor asociados.
        /// </returns>
        List<ActivosPrecio> FiltrarDatos(string cvsData);

        /// <summary>
        /// Verifica que los datos de precios cumplan con las reglas mínimas
        /// necesarias para ejecutar una predicción válida.
        /// 
        /// Reglas principales:
        /// - Deben existir al menos 20 registros
        /// - Los valores deben ser mayores que cero
        /// - Las fechas deben ser válidas
        /// </summary>
        /// <param name="precios">
        /// Lista de preci
