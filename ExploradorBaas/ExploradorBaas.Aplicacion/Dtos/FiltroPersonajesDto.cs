namespace ExploradorBaas.Aplicacion.Dtos;

public sealed class FiltroPersonajesDto
{
    public string? Nombre { get; set; }
    public string? Estado { get; set; }
    public string? Especie { get; set; }
    public int Pagina { get; set; } = 1;
}
