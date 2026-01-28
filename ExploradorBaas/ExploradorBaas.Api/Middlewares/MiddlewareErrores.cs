using System.Net;
using ExploradorBaas.Aplicacion.Errores;

namespace ExploradorBaas.Api.Middlewares;

public sealed class MiddlewareErrores
{
    private readonly RequestDelegate _next;

    public MiddlewareErrores(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NoEncontradoException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsJsonAsync(new { mensaje = ex.Message });
        }
        catch (ApiExternaException ex)
        {
            context.Response.StatusCode = ex.CodigoHttp ?? (int)HttpStatusCode.BadGateway;
            await context.Response.WriteAsJsonAsync(new { mensaje = ex.Message });
        }
        catch (Exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new { mensaje = "Error interno inesperado." });
        }
    }
}
