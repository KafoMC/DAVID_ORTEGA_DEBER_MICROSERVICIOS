using Catalogo.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Api.Data;

public sealed class CatalogoDbContext(DbContextOptions<CatalogoDbContext> options) : DbContext(options)
{
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Marca> Marcas => Set<Marca>();
    public DbSet<Producto> Productos => Set<Producto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>().ToTable("Categoria").HasKey(x => x.IdCategoria);
        modelBuilder.Entity<Marca>().ToTable("Marca").HasKey(x => x.IdMarca);
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("Producto");
            entity.HasKey(x => x.IdProducto);
            entity.HasIndex(x => x.SKU).IsUnique();
            entity.HasOne(x => x.Categoria).WithMany(x => x.Productos).HasForeignKey(x => x.IdCategoria).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Marca).WithMany(x => x.Productos).HasForeignKey(x => x.IdMarca).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
