using PredictorActivos.Models.Enums;

namespace PredictorActivos.Models.DTOs;

public class PredicModoDto
{
    public PredictionModo SeleccionModo { get; set; } = PredictionModo.Sma;
}