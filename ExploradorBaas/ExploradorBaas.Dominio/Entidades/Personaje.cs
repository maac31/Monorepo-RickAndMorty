
namespace ExploradorBaas.Dominio.Entidades;

public sealed class Personaje
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Especie { get; set; } = string.Empty;
    public string Ubicacion { get; set; } = string.Empty;
    public string ImagenUrl { get; set; } = string.Empty;

    public DateTime FechaActualizacionUtc { get; set; } = DateTime.UtcNow;

    public List<EpisodioPersonaje> Episodios { get; set; } = new();
}
