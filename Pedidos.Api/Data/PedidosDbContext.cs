using Microsoft.EntityFrameworkCore;
using Pedidos.Api.Models;

namespace Pedidos.Api.Data;

public sealed class PedidosDbContext(DbContextOptions<PedidosDbContext> options) : DbContext(options)
{
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<DetallePedido> DetallesPedido => Set<DetallePedido>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("Cliente", "dbo");
            entity.HasKey(cliente => cliente.IdCliente);
            entity.Property(cliente => cliente.IdCliente).ValueGeneratedOnAdd();
            entity.Property(cliente => cliente.Nombres).HasColumnType("varchar(150)").HasMaxLength(150).IsRequired();
            entity.Property(cliente => cliente.Cedula).HasColumnType("varchar(20)").HasMaxLength(20);
            entity.Property(cliente => cliente.Telefono).HasColumnType("varchar(20)").HasMaxLength(20);
            entity.Property(cliente => cliente.Correo).HasColumnType("varchar(100)").HasMaxLength(100);
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.ToTable("Pedido", "dbo");
            entity.HasKey(pedido => pedido.IdPedido);
            entity.Property(pedido => pedido.IdPedido).ValueGeneratedOnAdd();
            entity.Property(pedido => pedido.Fecha).HasColumnType("datetime2");
            entity.Property(pedido => pedido.Total).HasColumnType("decimal(12,2)");
            entity.Property(pedido => pedido.Estado).HasColumnType("varchar(30)").HasMaxLength(30);
            entity.HasOne(pedido => pedido.Cliente)
                .WithMany()
                .HasForeignKey(pedido => pedido.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.ToTable("DetallePedido", "dbo");
            entity.HasKey(detalle => detalle.IdDetalle);
            entity.Property(detalle => detalle.IdDetalle).ValueGeneratedOnAdd();
            entity.Property(detalle => detalle.PrecioUnitario).HasColumnType("decimal(10,2)");
            entity.HasOne(detalle => detalle.Pedido)
                .WithMany(pedido => pedido.Detalles)
                .HasForeignKey(detalle => detalle.IdPedido)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
