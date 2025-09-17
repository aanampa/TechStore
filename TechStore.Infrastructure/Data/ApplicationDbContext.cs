using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Domain.Entities;


namespace TechStore.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<DetalleOrden> DetallesOrden { get; set; }
        public DbSet<CarritoItem> CarritoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones de las entidades
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Precio).HasColumnType("decimal(18,2)");
                entity.Property(p => p.Categoria).HasMaxLength(50);
            });

            // Configuraciones similares para las demás entidades...
            // Configuración para Cliente
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Documento)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Apellido)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Direccion)
                    .HasMaxLength(200);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(20);

                entity.Property(e => e.FechaCreacion)
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");

                // Índices únicos
                entity.HasIndex(e => e.Email)
                    .IsUnique()
                    .HasDatabaseName("IX_Clientes_Email");

                entity.HasIndex(e => e.Documento)
                    .IsUnique()
                    .HasDatabaseName("IX_Clientes_Documento");

                // Relaciones
                entity.HasMany(c => c.CarritoItems)
                    .WithOne(ci => ci.Cliente)
                    .HasForeignKey(ci => ci.ClienteId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(c => c.Ordenes)
                    .WithOne(o => o.Cliente)
                    .HasForeignKey(o => o.ClienteId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.ToTable("Clientes");
            });
        }
    }
}