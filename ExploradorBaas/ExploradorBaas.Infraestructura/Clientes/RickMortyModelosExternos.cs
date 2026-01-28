using System.Text.Json.Serialization;

namespace ExploradorBaas.Infraestructura.Clientes;

public sealed class RespuestaPersonajesRickMorty
{
    [JsonPropertyName("info")]
    public InfoRickMorty? Info { get; set; }

    [JsonPropertyName("results")]
    public List<PersonajeRickMorty> Results { get; set; } = new();
}

public sealed class InfoRickMorty
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("pages")]
    public int Pages { get; set; }

    [JsonPropertyName("next")]
    public string? Next { get; set; }

    [JsonPropertyName("prev")]
    public string? Prev { get; set; }
}

public sealed class PersonajeRickMorty
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("species")]
    public string Species { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("gender")]
    public string Gender { get; set; } = string.Empty;

    [JsonPropertyName("origin")]
    public LugarRickMorty? Origin { get; set; }

    [JsonPropertyName("location")]
    public LugarRickMorty? Location { get; set; }

    [JsonPropertyName("image")]
    public string Image { get; set; } = string.Empty;

    [JsonPropertyName("episode")]
    public List<string> Episode { get; set; } = new();
}

public sealed class LugarRickMorty
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}

public sealed class EpisodioRickMorty
{
    [System.Text.Json.Serialization.JsonPropertyName("id")]
    public int Id { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [System.Text.Json.Serialization.JsonPropertyName("episode")]
    public string Episode { get; set; } = string.Empty;

    [System.Text.Json.Serialization.JsonPropertyName("air_date")]
    public string AirDate { get; set; } = string.Empty;
}
