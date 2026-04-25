using ConferenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConferenSpace.Infrastructure.Data;

public class ConferenSpaceDbContext : DbContext
{
    public ConferenSpaceDbContext(DbContextOptions<ConferenSpaceDbContext> options) : base(options)
    {
    }

    public DbSet<Salon> Salones { get; set; } = null!;
    public DbSet<Solicitante> Solicitantes { get; set; } = null!;
    public DbSet<Recurso> Recursos { get; set; } = null!;
    public DbSet<Reserva> Reservas { get; set; } = null!;
    public DbSet<ReservaRecurso> ReservaRecursos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Salon>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Ubicacion).IsRequired().HasMaxLength(500);
            entity.Property(e => e.CapacidadMaxima).IsRequired();
            entity.Property(e => e.ServiciosIntegrados).HasMaxLength(1000);
            entity.Property(e => e.EstaActivo).IsRequired().HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion).IsRequired().HasDefaultValueSql("GETUTCDATE()");

            entity.HasMany(e => e.Reservas)
                .WithOne(r => r.Salon)
                .HasForeignKey(r => r.SalonId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Solicitante>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NombreCompleto).IsRequired().HasMaxLength(300);
            entity.Property(e => e.Telefono).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Correo).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Departamento).IsRequired().HasMaxLength(200);
            entity.Property(e => e.NumeroIdentificacion).IsRequired().HasMaxLength(50);
            entity.Property(e => e.EstaActivo).IsRequired().HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion).IsRequired().HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => e.Correo).IsUnique();
            entity.HasIndex(e => e.NumeroIdentificacion).IsUnique();

            entity.HasMany(e => e.Reservas)
                .WithOne(r => r.Solicitante)
                .HasForeignKey(r => r.SolicitanteId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Recurso>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Descripcion).HasMaxLength(1000);
            entity.Property(e => e.CantidadTotal).IsRequired();
            entity.Property(e => e.CantidadDisponible).IsRequired();
            entity.Property(e => e.CostoUnitario).HasPrecision(10, 2);
            entity.Property(e => e.EstaActivo).IsRequired().HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion).IsRequired().HasDefaultValueSql("GETUTCDATE()");

            entity.HasMany(e => e.ReservaRecursos)
                .WithOne(rr => rr.Recurso)
                .HasForeignKey(rr => rr.RecursoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SolicitanteId).IsRequired();
            entity.Property(e => e.SalonId).IsRequired();
            entity.Property(e => e.Fecha).IsRequired();
            entity.Property(e => e.HoraInicio).IsRequired();
            entity.Property(e => e.HoraFin).IsRequired();
            entity.Property(e => e.Proposito).IsRequired().HasMaxLength(500);
            entity.Property(e => e.CantidadAsistentes).IsRequired();
            entity.Property(e => e.Estado).IsRequired().HasDefaultValue(EstadoReserva.Confirmada);
            entity.Property(e => e.Notas).HasMaxLength(1000);
            entity.Property(e => e.FechaCreacion).IsRequired().HasDefaultValueSql("GETUTCDATE()");

            entity.HasMany(e => e.ReservaRecursos)
                .WithOne(rr => rr.Reserva)
                .HasForeignKey(rr => rr.ReservaId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.SalonId, e.Fecha });
            entity.HasIndex(e => e.SolicitanteId);
            entity.HasIndex(e => e.Estado);
        });

        modelBuilder.Entity<ReservaRecurso>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ReservaId).IsRequired();
            entity.Property(e => e.RecursoId).IsRequired();
            entity.Property(e => e.CantidadSolicitada).IsRequired();
            entity.Property(e => e.FechaCreacion).IsRequired().HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => new { e.ReservaId, e.RecursoId }).IsUnique();
        });
    }
}
