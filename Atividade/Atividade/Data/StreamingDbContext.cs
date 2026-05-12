using Atividade.Models;
using Microsoft.EntityFrameworkCore;

namespace Atividade.Data;

public class StreamingDbContext(DbContextOptions<StreamingDbContext> options) : DbContext(options)
{
    public DbSet<ConteudoStreaming> Conteudos => Set<ConteudoStreaming>();

    public DbSet<PlanoStreaming> Planos => Set<PlanoStreaming>();

    public DbSet<AssinanteStreaming> Assinantes => Set<AssinanteStreaming>();

    public DbSet<PerfilStreaming> Perfis => Set<PerfilStreaming>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlanoStreaming>()
            .HasMany(plano => plano.Assinantes)
            .WithOne(assinante => assinante.Plano)
            .HasForeignKey(assinante => assinante.PlanoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AssinanteStreaming>()
            .HasMany(assinante => assinante.Perfis)
            .WithOne(perfil => perfil.Assinante)
            .HasForeignKey(perfil => perfil.AssinanteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AssinanteStreaming>()
            .HasIndex(assinante => assinante.Email)
            .IsUnique();

        modelBuilder.Entity<PlanoStreaming>()
            .HasIndex(plano => plano.Nome)
            .IsUnique();
    }
}
