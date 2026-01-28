using ExploradorBaas.Aplicacion.Dtos;

namespace ExploradorBaas.Aplicacion.Contratos;

public interface IRickMortyRepositorio
{
    Task<PaginaPersonajesDto> ObtenerPersonajesAsync(FiltroPersonajesDto filtro, CancellationToken cancellationToken);
    Task<PersonajeDetalleDto> ObtenerPersonajePorIdAsync(int id, CancellationToken cancellationToken);
    Task<List<EpisodioDto>> ObtenerEpisodiosDePersonajeAsync(int personajeId, CancellationToken cancellationToken);

}
