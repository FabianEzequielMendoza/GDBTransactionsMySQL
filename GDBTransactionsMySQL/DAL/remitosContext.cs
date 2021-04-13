using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace GDBTransactionsMySQL.DAL
{
    public partial class remitosContext : DbContext
    {
        public remitosContext()
        {
        }

        public remitosContext(DbContextOptions<remitosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Articulo> Articulos { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Remito> Remitos { get; set; }
        public virtual DbSet<RemitosDetalle> RemitosDetalles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseMySQL("server=localhost;port=3307;user=root;password=usbw;database=remitos");
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Articulo>(entity =>
            {
                entity.HasKey(e => e.IdArticulo)
                    .HasName("PRIMARY");

                entity.ToTable("articulos");

                entity.Property(e => e.IdArticulo)
                    .HasColumnType("mediumint(9)")
                    .HasColumnName("ID_ARTICULO");

                entity.Property(e => e.Articulo1)
                    .HasMaxLength(255)
                    .HasColumnName("ARTICULO");

                entity.Property(e => e.Precio)
                    .HasColumnType("mediumint(9)")
                    .HasColumnName("PRECIO");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente)
                    .HasName("PRIMARY");

                entity.ToTable("clientes");

                entity.Property(e => e.IdCliente)
                    .HasColumnType("mediumint(9)")
                    .HasColumnName("id_cliente");

                entity.Property(e => e.Dni)
                    .HasColumnType("int(9)")
                    .HasColumnName("dni");

                entity.Property(e => e.NombreApellido)
                    .HasMaxLength(255)
                    .HasColumnName("nombre_apellido");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(100)
                    .HasColumnName("telefono");
            });

            modelBuilder.Entity<Remito>(entity =>
            {
                entity.HasKey(e => e.IdRemito)
                    .HasName("PRIMARY");

                entity.ToTable("remitos");

                entity.HasIndex(e => e.IdCliente, "ID_CLIENTE_2");

                entity.HasIndex(e => e.IdCliente, "fk_clientes_idx");

                entity.HasIndex(e => e.IdCliente, "id_cliente");

                entity.HasIndex(e => e.IdCliente, "remito_id_cliente");

                entity.Property(e => e.IdRemito)
                    .HasColumnType("mediumint(9)")
                    .HasColumnName("ID_REMITO");

                entity.Property(e => e.Fecha)
                    .HasColumnType("date")
                    .HasColumnName("FECHA");

                entity.Property(e => e.IdCliente)
                    .HasColumnType("mediumint(9)")
                    .HasColumnName("ID_CLIENTE");

                entity.Property(e => e.MontoTotal)
                    .HasColumnType("decimal(10,0)")
                    .HasColumnName("MONTO_TOTAL");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Remitos)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_clientes");
            });

            modelBuilder.Entity<RemitosDetalle>(entity =>
            {
                entity.HasKey(e => e.IdDetalle)
                    .HasName("PRIMARY");

                entity.ToTable("remitos_detalle");

                entity.HasIndex(e => e.IdArticulo, "ID_ARTICULO");

                entity.HasIndex(e => e.IdRemito, "ID_REMITO");

                entity.Property(e => e.IdDetalle)
                    .HasColumnType("mediumint(8) unsigned")
                    .HasColumnName("id_detalle");

                entity.Property(e => e.Cantidad)
                    .HasColumnType("mediumint(9)")
                    .HasColumnName("CANTIDAD");

                entity.Property(e => e.IdArticulo)
                    .HasColumnType("mediumint(9)")
                    .HasColumnName("ID_ARTICULO");

                entity.Property(e => e.IdRemito)
                    .HasColumnType("mediumint(9)")
                    .HasColumnName("ID_REMITO");

                entity.Property(e => e.MontoItem)
                    .HasColumnType("mediumint(9)")
                    .HasColumnName("MONTO_ITEM");

                entity.HasOne(d => d.IdArticuloNavigation)
                    .WithMany(p => p.RemitosDetalles)
                    .HasForeignKey(d => d.IdArticulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_art");

                entity.HasOne(d => d.IdRemitoNavigation)
                    .WithMany(p => p.RemitosDetalles)
                    .HasForeignKey(d => d.IdRemito)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("remitos_detalle_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
