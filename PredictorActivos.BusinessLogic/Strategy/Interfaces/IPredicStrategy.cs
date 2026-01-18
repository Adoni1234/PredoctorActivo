using PredictorActivos.Models.DTO;

namespace PredictorActivos.Models.Strategy;

public interface IPredicStrategy
{
    PredictionResultDto CalculoPrediction(List<ActivosPrecio> precios);
    string NombreModo { get; }
}