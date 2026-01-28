namespace ExploradorBaas.Aplicacion.Dtos;

public sealed class PersonajeDetalleDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Especie { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Genero { get; set; } = string.Empty;

    public string Origen { get; set; } = string.Empty;
    public string Ubicacion { get; set; } = string.Empty;

    public string ImagenUrl { get; set; } = string.Empty;

     
    public List<string> Episodios { get; set; } = new();
}
