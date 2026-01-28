namespace ExploradorBaas.Dominio.Entidades;

public sealed class EpisodioPersonaje
{
    public int Id { get; set; }

    public int PersonajeId { get; set; }
    public Personaje? Personaje { get; set; }

    public string UrlEpisodio { get; set; } = string.Empty;
}
