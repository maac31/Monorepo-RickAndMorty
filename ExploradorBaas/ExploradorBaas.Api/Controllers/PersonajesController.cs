using ExploradorBaas.Aplicacion.Contratos;
using ExploradorBaas.Aplicacion.Dtos;
using Microsoft.AspNetCore.Mvc;


namespace ExploradorBaas.Api.Controllers;

[ApiController]
[Route("api/personajes")]
public sealed class PersonajesController : ControllerBase
{
    private readonly IRickMortyRepositorio _rickMortyRepositorio;

    public PersonajesController(IRickMortyRepositorio rickMortyRepositorio)
    {
        _rickMortyRepositorio = rickMortyRepositorio;
    }

    
    [HttpGet]
    public async Task<ActionResult<PaginaPersonajesDto>> ObtenerListado(
        [FromQuery] string? nombre,
        [FromQuery] string? estado,
        [FromQuery] string? especie,
        [FromQuery] int pagina = 1,
        CancellationToken cancellationToken = default)
    {
        var filtro = new FiltroPersonajesDto
        {
            Nombre = nombre,
            Estado = estado,
            Especie = especie,
            Pagina = pagina
        };

        var resultado = await _rickMortyRepositorio.ObtenerPersonajesAsync(filtro, cancellationToken);
        return Ok(resultado);
    }

    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PersonajeDetalleDto>> ObtenerDetalle(
        [FromRoute] int id,
        CancellationToken cancellationToken = default)
    {
        var resultado = await _rickMortyRepositorio.ObtenerPersonajePorIdAsync(id, cancellationToken);
        return Ok(resultado);
    }

    
    [HttpGet("{id:int}/episodios")]
    public async Task<ActionResult<List<EpisodioDto>>> ObtenerEpisodios(
        [FromRoute] int id,
        CancellationToken cancellationToken = default)
    {
        var resultado = await _rickMortyRepositorio.ObtenerEpisodiosDePersonajeAsync(id, cancellationToken);
        return Ok(resultado);
    }


}
