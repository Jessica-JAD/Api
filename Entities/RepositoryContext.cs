using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Operador> Operadores { get; set; }

        public DbSet<Articulo> Articulos { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        public DbSet<StockLinea> StocksLinea { get; set; }

        public DbSet<StockLineaDetalle> StocksLineasDetalles { get; set; }

        public DbSet<UnidadMedida> UnidadesMedida { get; set; }

        public DbSet<AlmacenSeccion> AlmacenSecciones { get; set; }

        public DbSet<Almacen> Almacenes { get; set; }

        public DbSet<Seccion> Secciones { get; set; }

        public DbSet<Moneda> Monedas { get; set; }

        public DbSet<EmpresaMoneda> EmpresaMonedas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Almacen
            modelBuilder.Entity<Almacen>()
                .HasIndex(a => new { a.Codigo })
                .IsUnique();

            //Seccion
            modelBuilder.Entity<Seccion>()
                .HasIndex(a => new { a.Codigo })
                .IsUnique();

            //Almacen/Seccion
            modelBuilder.Entity<AlmacenSeccion>()
                .Property(ras => ras.IdTipoSeccion)
                .HasConversion(v => ((int)v).ToString(), v => (ETiposSeccion)Enum.Parse(typeof(ETiposSeccion), v));

            //Unidad de Medida
            modelBuilder.Entity<UnidadMedida>()
                .HasIndex(a => new { a.Codigo })
                .IsUnique();
            
            //Articulo
            modelBuilder.Entity<Articulo>()
                .HasIndex(a => new { a.Codigo })
                .IsUnique();

            //Stock
            modelBuilder.Entity<Stock>()
                .Property(st => st.Maximo)
                .HasPrecision(18, 4);
            modelBuilder.Entity<Stock>()
                .Property(st => st.Minimo)
                .HasPrecision(18, 4);

            //Stock-Linea
            modelBuilder.Entity<StockLinea>()
                .Property(l => l.Existencia)
                .HasPrecision(18, 4);

            //Stock-Linea/Detalle
            modelBuilder.Entity<StockLineaDetalle>()
                .HasKey(d => new { d.StockLineaId, d.EmpresaMonedaId});
            modelBuilder.Entity<StockLineaDetalle>()
                .Property(d => d.Precio)
                .HasPrecision(18, 8);
            modelBuilder.Entity<StockLineaDetalle>()
                .Property(d => d.Importe)
                .HasPrecision(15, 2);

            //Stock-Precio
            modelBuilder.Entity<StockPrecio>()
                .HasKey(p => new { p.StockId, p.EmpresaMonedaId });
            modelBuilder.Entity<StockPrecio>()
                .Property(p => p.Precio)
                .HasPrecision(18, 8);
            modelBuilder.Entity<StockPrecio>()
                .Property(p => p.PrecioVenta)
                .HasPrecision(15, 4);

            // Solicitud
            modelBuilder.Entity<Solicitud>()
                .Property(s => s.IdEstado)
                .HasConversion(v => ((byte)v).ToString(), v => (EPedidosEstados)Enum.Parse(typeof(EPedidosEstados), v)); // Para q almacene en formato "char"

            // Solicitud-Linea
            modelBuilder.Entity<SolicitudLinea>()
                .Property(l => l.Cantidad)
                .HasPrecision(18, 12);
            modelBuilder.Entity<SolicitudLinea>()
                .Property(l => l.CantidadDestino)
                .HasPrecision(18, 12);
            modelBuilder.Entity<SolicitudLinea>()
                .HasIndex(l => new { l.SolicitudId, l.ArticuloId })
                .IsUnique();

            // Solicitud-Linea-Detalle
            modelBuilder.Entity<SolicitudLineaDetalle>()
                .HasKey(ld => new { ld.SolicitudLineaId, ld.EmpresaMonedaId});
            modelBuilder.Entity<SolicitudLineaDetalle>()
                .Property(ld => ld.Precio )
                .HasPrecision(18, 8);
            modelBuilder.Entity<SolicitudLineaDetalle>()
                .Property(ld => ld.Importe)
                .HasPrecision(10, 2);
            modelBuilder.Entity<SolicitudLineaDetalle>()
                .Property(ld => ld.PrecioDestino)
                .HasPrecision(18, 8);
            modelBuilder.Entity<SolicitudLineaDetalle>()
                .Property(ld => ld.ImporteDestino)
                .HasPrecision(10, 2);

            base.OnModelCreating(modelBuilder);
        }

    }
}
