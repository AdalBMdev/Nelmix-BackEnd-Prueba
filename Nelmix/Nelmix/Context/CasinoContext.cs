using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Nelmix.Models;

namespace Nelmix.Context
{
    public partial class CasinoContext : DbContext
    {
        public CasinoContext()
        {
        }

        public CasinoContext(DbContextOptions<CasinoContext> options) : base(options)
        {
        }

        public virtual DbSet<ApuestasUsuario> ApuestasUsuarios { get; set; } = null!;
        public virtual DbSet<CuentasBancaria> CuentasBancarias { get; set; } = null!;
        public virtual DbSet<EstadosUsuario> EstadosUsuarios { get; set; } = null!;
        public virtual DbSet<Ficha> Fichas { get; set; } = null!;
        public virtual DbSet<Juego> Juegos { get; set; } = null!;
        public virtual DbSet<Moneda> Monedas { get; set; } = null!;
        public virtual DbSet<TasasDeCambio> TasasDeCambios { get; set; } = null!;
        public virtual DbSet<TiposDeFicha> TiposDeFichas { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApuestasUsuario>(entity =>
            {
                entity.HasKey(e => e.ApuestaId)
                    .HasName("PK__Apuestas__10471BC3CCD62704");

                entity.ToTable("ApuestasUsuario");

                entity.Property(e => e.ApuestaId).HasColumnName("apuesta_id");

                entity.Property(e => e.CantidadGanada)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("cantidad_ganada");

                entity.Property(e => e.CantidadPerdida)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("cantidad_perdida");

                entity.Property(e => e.JuegoId).HasColumnName("juego_id");

                entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            });

            modelBuilder.Entity<CuentasBancaria>(entity =>
            {
                entity.HasKey(e => e.CuentaId)
                    .HasName("PK__CuentasB__612B08615BC794D6");

                entity.Property(e => e.CuentaId).HasColumnName("cuenta_id");

                entity.Property(e => e.MonedaId).HasColumnName("moneda_id");

                entity.Property(e => e.Saldo)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("saldo");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<EstadosUsuario>(entity =>
            {
                entity.HasKey(e => e.EstadoId)
                    .HasName("PK__EstadosU__053774EF51EC907B");

                entity.ToTable("EstadosUsuario");

                entity.Property(e => e.EstadoId).HasColumnName("estado_id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Ficha>(entity =>
            {
                entity.Property(e => e.FichaId).HasColumnName("ficha_id");

                entity.Property(e => e.CantidadDisponible).HasColumnName("cantidad_disponible");

                entity.Property(e => e.TipoFichaId).HasColumnName("tipo_ficha_id");

                entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            });

            modelBuilder.Entity<Juego>(entity =>
            {
                entity.Property(e => e.JuegoId).HasColumnName("juego_id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Moneda>(entity =>
            {
                entity.Property(e => e.MonedaId).HasColumnName("moneda_id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .HasColumnName("nombre");

                entity.Property(e => e.Símbolo)
                    .HasMaxLength(3)
                    .HasColumnName("símbolo");
            });

            modelBuilder.Entity<TasasDeCambio>(entity =>
            {
                entity.HasKey(e => e.TasaId)
                    .HasName("PK__TasasDeC__9E6E85019314EC26");

                entity.ToTable("TasasDeCambio");

                entity.Property(e => e.TasaId).HasColumnName("tasa_id");

                entity.Property(e => e.MonedaId).HasColumnName("moneda_id");

                entity.Property(e => e.Tasa)
                    .HasColumnType("decimal(10, 5)")
                    .HasColumnName("tasa");

                entity.HasOne(d => d.Moneda)
                    .WithMany(p => p.TasasDeCambios)
                    .HasForeignKey(d => d.MonedaId)
                    .HasConstraintName("FK__TasasDeCa__moned__6A30C649");
            });

            modelBuilder.Entity<TiposDeFicha>(entity =>
            {
                entity.HasKey(e => e.TipoFichaId)
                    .HasName("PK__TiposDeF__10181B7DEFFB9A4E");

                entity.Property(e => e.TipoFichaId).HasColumnName("tipo_ficha_id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .HasColumnName("nombre");

                entity.Property(e => e.Valor).HasColumnName("valor");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__Usuarios__B9BE370FCFF60292");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.AdultoAsignadoId).HasColumnName("adulto_asignado_id");

                entity.Property(e => e.Contraseña)
                    .HasMaxLength(255)
                    .HasColumnName("contraseña");

                entity.Property(e => e.Edad).HasColumnName("edad");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.EstadoId).HasColumnName("estado_id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .HasColumnName("nombre");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
