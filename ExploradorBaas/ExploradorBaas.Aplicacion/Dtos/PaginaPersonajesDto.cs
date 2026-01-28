namespace ExploradorBaas.Aplicacion.Dtos;

public sealed class PaginaPersonajesDto
{
    public int TotalRegistros { get; set; }
    public int TotalPaginas { get; set; }
    public int PaginaActual { get; set; }
    public List<PersonajeResumenDto> Resultados { get; set; } = new();
}
