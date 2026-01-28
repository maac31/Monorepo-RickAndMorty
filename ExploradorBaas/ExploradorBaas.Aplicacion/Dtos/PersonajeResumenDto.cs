namespace ExploradorBaas.Aplicacion.Dtos;

public sealed class PersonajeResumenDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Especie { get; set; } = string.Empty;
    public string Ubicacion { get; set; } = string.Empty;
    public string ImagenUrl { get; set; } = string.Empty;
}
