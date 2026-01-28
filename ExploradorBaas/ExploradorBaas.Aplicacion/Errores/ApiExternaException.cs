namespace ExploradorBaas.Aplicacion.Errores;

public sealed class ApiExternaException : Exception
{
    public int? CodigoHttp { get; }

    public ApiExternaException(string mensaje, int? codigoHttp = null, Exception? inner = null)
        : base(mensaje, inner)
    {
        CodigoHttp = codigoHttp;
    }
}
