# ExploradorBaas (Backend) - Rick & Morty API

Backend en ASP.NET Core (.NET 8) que consume la Rick & Morty API, expone endpoints propios y persiste información relevante en MySQL usando EF Core.

## Requisitos
- .NET SDK 8 instalado
- MySQL 8.x instalado y corriendo
- PowerShell (Windows)

## Arquitectura (capas)
- **ExploradorBaas.Api**: Controllers, configuración, Swagger y middleware de errores.
- **ExploradorBaas.Aplicacion**: DTOs, contratos (interfaces) y excepciones de negocio.
- **ExploradorBaas.Dominio**: Entidades del dominio (Personaje, EpisodioPersonaje).
- **ExploradorBaas.Infraestructura**: consumo de API externa (HttpClient), persistencia (DbContext) y repositorios.

## Configuración
En `ExploradorBaas.Api/appsettings.Development.json` configura tu cadena de conexión:

```json
{
  "ConnectionStrings": {
    "MySql": "server=localhost;port=3306;database=explorador_baas;user=root;password=PASSWORD;"
  }
}
