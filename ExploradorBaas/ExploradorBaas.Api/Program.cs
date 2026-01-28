using ExploradorBaas.Api.Middlewares;
using ExploradorBaas.Aplicacion.Contratos;
using ExploradorBaas.Infraestructura.Clientes;
using ExploradorBaas.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? cadenaMySql = builder.Configuration.GetConnectionString("MySql");
builder.Services.AddDbContext<ExploradorBaasDbContext>(opciones =>
{
    opciones.UseMySql(cadenaMySql, ServerVersion.AutoDetect(cadenaMySql));
});

// HttpClient tipado para la API externa
builder.Services.AddHttpClient<IRickMortyRepositorio, RickMortyRepositorio>(cliente =>
{
    cliente.BaseAddress = new Uri("https://rickandmortyapi.com/api/");
    cliente.Timeout = TimeSpan.FromSeconds(15);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<MiddlewareErrores>();

app.MapControllers();

app.Run();
