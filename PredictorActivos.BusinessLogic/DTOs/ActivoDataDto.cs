using PredictorActivos.Models.Enums;

namespace PredictorActivos.Models.DTO;

public class ActivoDataDto
{
    public List<ActivosPrecio> Precios { get; set; } = new();
    public PredictionModo PredictionMode { get; set; }
}