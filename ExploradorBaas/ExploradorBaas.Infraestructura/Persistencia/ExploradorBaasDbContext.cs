using ExploradorBaas.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ExploradorBaas.Infraestructura.Persistencia;

public sealed class ExploradorBaasDbContext : DbContext
{
    public ExploradorBaasDbContext(DbContextOptions<ExploradorBaasDbContext> options) : base(options) { }

    public DbSet<Personaje> Personajes => Set<Personaje>();
    public DbSet<EpisodioPersonaje> EpisodiosPersonaje => Set<EpisodioPersonaje>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Personaje>(entidad =>
        {
            entidad.ToTable("personajes");
            entidad.HasKey(x => x.Id);

            entidad.Property(x => x.Nombre).HasMaxLength(200).IsRequired();
            entidad.Property(x => x.Estado).HasMaxLength(50).IsRequired();
            entidad.Property(x => x.Especie).HasMaxLength(100).IsRequired();
            entidad.Property(x => x.Ubicacion).HasMaxLength(200).IsRequired();
            entidad.Property(x => x.ImagenUrl).HasMaxLength(500).IsRequired();
            entidad.Property(x => x.Id).ValueGeneratedNever();


            entidad.Property(x => x.FechaActualizacionUtc).IsRequired();

            entidad.HasMany(x => x.Episodios)
                   .WithOne(x => x.Personaje!)
                   .HasForeignKey(x => x.PersonajeId)
                   .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<EpisodioPersonaje>(entidad =>
        {
            entidad.ToTable("episodios_personaje");
            entidad.HasKey(x => x.Id);

            entidad.Property(x => x.UrlEpisodio).HasMaxLength(500).IsRequired();
        });
    }
}
