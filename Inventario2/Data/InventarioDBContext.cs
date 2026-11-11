using Inventario.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Data;

public sealed class InventarioDBContext(DbContextOptions<InventarioDBContext> options) : DbContext(options)
{
    public DbSet<Bodega> Bodegas => Set<Bodega>();
    public DbSet<Existencia> Existencias => Set<Existencia>();
    public DbSet<MovimientoInventario> MovimientosInventario => Set<MovimientoInventario>();
    public DbSet<ReservaStock> ReservasStock => Set<ReservaStock>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bodega>().ToTable("Bodega").HasKey(x => x.IdBodega);
        modelBuilder.Entity<Existencia>(entity =>
        {
            entity.ToTable("Existencia"); entity.HasKey(x => x.IdExistencia);
            entity.HasAlternateKey(x => new { x.IdProducto, x.IdBodega });
            entity.HasOne(x => x.Bodega).WithMany(x => x.Existencias).HasForeignKey(x => x.IdBodega).OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<MovimientoInventario>(entity =>
        {
            entity.ToTable("MovimientoInventario"); entity.HasKey(x => x.IdMovimiento);
            entity.HasOne(x => x.Existencia).WithMany(x => x.Movimientos).HasForeignKey(x => new { x.IdProducto, x.IdBodega }).HasPrincipalKey(x => new { x.IdProducto, x.IdBodega }).OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<ReservaStock>(entity =>
        {
            entity.ToTable("ReservaStock"); entity.HasKey(x => x.IdReserva);
            entity.HasOne(x => x.Existencia).WithMany().HasForeignKey(x => new { x.IdProducto, x.IdBodega }).HasPrincipalKey(x => new { x.IdProducto, x.IdBodega }).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
