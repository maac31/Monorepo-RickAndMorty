namespace ExploradorBaas.Aplicacion.Dtos;

public sealed class EpisodioDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;      
    public string FechaEmision { get; set; } = string.Empty; 
}
