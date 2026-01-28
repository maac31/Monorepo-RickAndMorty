using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ExploradorBaas.Aplicacion.Contratos;
using ExploradorBaas.Aplicacion.Dtos;
using ExploradorBaas.Aplicacion.Errores;
using ExploradorBaas.Dominio.Entidades;
using ExploradorBaas.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;


namespace ExploradorBaas.Infraestructura.Clientes;

public sealed class RickMortyRepositorio : IRickMortyRepositorio
{
    private readonly HttpClient _httpClient;
    private readonly ExploradorBaasDbContext _dbContext;

    private static readonly JsonSerializerOptions OpcionesJson = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public RickMortyRepositorio(HttpClient httpClient, ExploradorBaasDbContext dbContext)
    {
        _httpClient = httpClient;
        _dbContext = dbContext;
    }

    public async Task<PaginaPersonajesDto> ObtenerPersonajesAsync(FiltroPersonajesDto filtro, CancellationToken cancellationToken)
    {
        int pagina = filtro.Pagina <= 0 ? 1 : filtro.Pagina;
        string url = ConstruirUrlListado(filtro, pagina);

        HttpResponseMessage respuesta;
        try
        {
            respuesta = await _httpClient.GetAsync(url, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApiExternaException("Error consultando la API externa (Rick & Morty).", null, ex);
        }

        
        if (respuesta.StatusCode == HttpStatusCode.NotFound)
        {
            return new PaginaPersonajesDto
            {
                TotalRegistros = 0,
                TotalPaginas = 0,
                PaginaActual = pagina,
                Resultados = new List<PersonajeResumenDto>()
            };
        }

        if (!respuesta.IsSuccessStatusCode)
        {
            throw new ApiExternaException(
                $"La API externa respondió con error {(int)respuesta.StatusCode}.",
                (int)respuesta.StatusCode
            );
        }

        var data = await respuesta.Content.ReadFromJsonAsync<RespuestaPersonajesRickMorty>(OpcionesJson, cancellationToken);

        var paginaDto = new PaginaPersonajesDto
        {
            TotalRegistros = data?.Info?.Count ?? 0,
            TotalPaginas = data?.Info?.Pages ?? 0,
            PaginaActual = pagina
        };

        foreach (var p in data?.Results ?? new List<PersonajeRickMorty>())
        {
            paginaDto.Resultados.Add(new PersonajeResumenDto
            {
                Id = p.Id,
                Nombre = p.Name,
                Estado = p.Status,
                Especie = p.Species,
                Ubicacion = p.Location?.Name ?? string.Empty,
                ImagenUrl = p.Image
            });
        }

        // Guardado simple en DB 
        await GuardarPersonajesEnDbAsync(data?.Results ?? new List<PersonajeRickMorty>(), cancellationToken);

        return paginaDto;
    }

    public async Task<PersonajeDetalleDto> ObtenerPersonajePorIdAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) throw new NoEncontradoException("Id inválido.");

        HttpResponseMessage respuesta;
        try
        {
            respuesta = await _httpClient.GetAsync($"character/{id}", cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApiExternaException("Error consultando la API externa (Rick & Morty).", null, ex);
        }

        if (respuesta.StatusCode == HttpStatusCode.NotFound)
            throw new NoEncontradoException("Personaje no encontrado.");

        if (!respuesta.IsSuccessStatusCode)
        {
            throw new ApiExternaException(
                $"La API externa respondió con error {(int)respuesta.StatusCode}.",
                (int)respuesta.StatusCode
            );
        }

        var p = await respuesta.Content.ReadFromJsonAsync<PersonajeRickMorty>(OpcionesJson, cancellationToken);

        if (p is null)
            throw new ApiExternaException("Respuesta inválida desde la API externa.");

        var detalleDto = new PersonajeDetalleDto
        {
            Id = p.Id,
            Nombre = p.Name,
            Estado = p.Status,
            Especie = p.Species,
            Tipo = p.Type,
            Genero = p.Gender,
            Origen = p.Origin?.Name ?? string.Empty,
            Ubicacion = p.Location?.Name ?? string.Empty,
            ImagenUrl = p.Image,
            Episodios = p.Episode ?? new List<string>()
        };

        // Guardado con episodios en DB 
        await GuardarDetalleEnDbAsync(p, cancellationToken);

        return detalleDto;
    }

    private static string ConstruirUrlListado(FiltroPersonajesDto filtro, int pagina)
    {
        // Endpoint externo
        var parametros = new List<string> { $"page={pagina}" };

        if (!string.IsNullOrWhiteSpace(filtro.Nombre))
            parametros.Add($"name={Uri.EscapeDataString(filtro.Nombre)}");

        if (!string.IsNullOrWhiteSpace(filtro.Estado))
            parametros.Add($"status={Uri.EscapeDataString(filtro.Estado)}");

        if (!string.IsNullOrWhiteSpace(filtro.Especie))
            parametros.Add($"species={Uri.EscapeDataString(filtro.Especie)}");

        return $"character/?{string.Join("&", parametros)}";
    }

    private async Task GuardarPersonajesEnDbAsync(List<PersonajeRickMorty> personajes, CancellationToken cancellationToken)
    {
        foreach (var p in personajes)
        {
            var personajeDb = await _dbContext.Personajes
                .FirstOrDefaultAsync(x => x.Id == p.Id, cancellationToken);

            if (personajeDb is null)
            {
                personajeDb = new Personaje { Id = p.Id };
                _dbContext.Personajes.Add(personajeDb);
            }

            personajeDb.Nombre = p.Name;
            personajeDb.Estado = p.Status;
            personajeDb.Especie = p.Species;
            personajeDb.Ubicacion = p.Location?.Name ?? string.Empty;
            personajeDb.ImagenUrl = p.Image;
            personajeDb.FechaActualizacionUtc = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task GuardarDetalleEnDbAsync(PersonajeRickMorty p, CancellationToken cancellationToken)
    {
        var personajeDb = await _dbContext.Personajes
            .Include(x => x.Episodios)
            .FirstOrDefaultAsync(x => x.Id == p.Id, cancellationToken);

        if (personajeDb is null)
        {
            personajeDb = new Personaje { Id = p.Id };
            _dbContext.Personajes.Add(personajeDb);
        }

        personajeDb.Nombre = p.Name;
        personajeDb.Estado = p.Status;
        personajeDb.Especie = p.Species;
        personajeDb.Ubicacion = p.Location?.Name ?? string.Empty;
        personajeDb.ImagenUrl = p.Image;
        personajeDb.FechaActualizacionUtc = DateTime.UtcNow;

        // Reemplazar episodios
        personajeDb.Episodios.Clear();

        foreach (var urlEpisodio in p.Episode ?? new List<string>())
        {
            personajeDb.Episodios.Add(new EpisodioPersonaje
            {
                UrlEpisodio = urlEpisodio
            });
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<EpisodioDto>> ObtenerEpisodiosDePersonajeAsync(int personajeId, CancellationToken cancellationToken)
{
    // Primero obtengo el personaje (así recupero las URLs de episodios)
    HttpResponseMessage respuestaPersonaje;
    try
    {
        respuestaPersonaje = await _httpClient.GetAsync($"character/{personajeId}", cancellationToken);
    }
    catch (Exception ex)
    {
        throw new ApiExternaException("Error consultando la API externa (Rick & Morty).", null, ex);
    }

    if (respuestaPersonaje.StatusCode == HttpStatusCode.NotFound)
        throw new NoEncontradoException("Personaje no encontrado.");

    if (!respuestaPersonaje.IsSuccessStatusCode)
        throw new ApiExternaException($"La API externa respondió con error {(int)respuestaPersonaje.StatusCode}.", (int)respuestaPersonaje.StatusCode);

    var personaje = await respuestaPersonaje.Content.ReadFromJsonAsync<PersonajeRickMorty>(OpcionesJson, cancellationToken);

    var urls = personaje?.Episode ?? new List<string>();
    if (urls.Count == 0) return new List<EpisodioDto>();

    // Extraer ids desde URLs
    var ids = urls
        .Select(ExtraerIdEpisodioDesdeUrl)
        .Where(id => id > 0)
        .Distinct()
        .ToList();

    if (ids.Count == 0) return new List<EpisodioDto>();

    // Rick & Morty API permite pedir varios episodios en una sola llamada:
    // /episode/[1,2,3]
    string rutaEpisodios = ids.Count == 1
        ? $"episode/{ids[0]}"
        : $"episode/[{string.Join(",", ids)}]";

    HttpResponseMessage respuestaEpisodios;
    try
    {
        respuestaEpisodios = await _httpClient.GetAsync(rutaEpisodios, cancellationToken);
    }
    catch (Exception ex)
    {
        throw new ApiExternaException("Error consultando episodios en la API externa.", null, ex);
    }

    if (!respuestaEpisodios.IsSuccessStatusCode)
        throw new ApiExternaException($"La API externa respondió con error {(int)respuestaEpisodios.StatusCode}.", (int)respuestaEpisodios.StatusCode);

    // Si pides 1 id -> objeto, si pides varios -> arreglo
    if (ids.Count == 1)
    {
        var e = await respuestaEpisodios.Content.ReadFromJsonAsync<EpisodioRickMorty>(OpcionesJson, cancellationToken);
        if (e is null) return new List<EpisodioDto>();

        return new List<EpisodioDto>
        {
            new EpisodioDto { Id = e.Id, Nombre = e.Name, Codigo = e.Episode, FechaEmision = e.AirDate }
        };
    }

    var lista = await respuestaEpisodios.Content.ReadFromJsonAsync<List<EpisodioRickMorty>>(OpcionesJson, cancellationToken)
                ?? new List<EpisodioRickMorty>();

    return lista
        .Select(e => new EpisodioDto { Id = e.Id, Nombre = e.Name, Codigo = e.Episode, FechaEmision = e.AirDate })
        .OrderBy(e => e.Id)
        .ToList();
}

private static int ExtraerIdEpisodioDesdeUrl(string url)
{
    // Ejemplo: https://rickandmortyapi.com/api/episode/28
    if (string.IsNullOrWhiteSpace(url)) return 0;

    var partes = url.TrimEnd('/').Split('/');
    var ultimo = partes.LastOrDefault();
    return int.TryParse(ultimo, out int id) ? id : 0;
}

}
