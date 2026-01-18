namespace PredictorActivos.Models.DTO;

public class PredictionResultDto
{
    public string Modo { get; set; } = string.Empty;
    public string Tendencia { get; set; } = string.Empty;
    public string Detalles { get; set; } = string.Empty;
    public decimal? ValorFuturo { get; set; }
    public List<string> Calculos { get; set; } = new();
}