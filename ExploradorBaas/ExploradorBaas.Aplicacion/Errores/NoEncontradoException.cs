namespace ExploradorBaas.Aplicacion.Errores;

public sealed class NoEncontradoException : Exception
{
    public NoEncontradoException(string mensaje) : base(mensaje) { }
}
