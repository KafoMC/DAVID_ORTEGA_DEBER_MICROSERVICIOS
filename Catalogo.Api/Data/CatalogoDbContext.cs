using Microsoft.EntityFrameworkCore;
using Catalogo.Api.Models;
namespace Catalogo.Api.Data;
public sealed class CatalogoDbContext(DbContextOptions<CatalogoDbContext> options):DbContext(options)
{
 public DbSet<Producto> Productos=>Set<Producto>(); public DbSet<Categoria> Categorias=>Set<Categoria>(); public DbSet<Marca> Marcas=>Set<Marca>();
 protected override void OnModelCreating(ModelBuilder b){b.Entity<Producto>(e=>{e.ToTable("Productos");e.HasKey(x=>x.IdProducto);e.Property(x=>x.Precio).HasPrecision(10,2);e.HasOne<Categoria>().WithMany().HasForeignKey(x=>x.IdCategoria);e.HasOne<Marca>().WithMany().HasForeignKey(x=>x.IdMarca);});b.Entity<Categoria>().ToTable("Categoria").HasKey(x=>x.IdCategoria);b.Entity<Marca>().ToTable("Marca").HasKey(x=>x.IdMarca);}
}
