using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedidos.Api.Data;
using Pedidos.Api.Dtos;
using Pedidos.Api.Models;

namespace Pedidos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PedidosController(PedidosDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos(CancellationToken cancellationToken)
    {
        var pedidos = await context.Pedidos
            .AsNoTracking()
            .Include(pedido => pedido.Cliente)
            .Include(pedido => pedido.Detalles)
            .OrderByDescending(pedido => pedido.Fecha)
            .ThenByDescending(pedido => pedido.IdPedido)
            .ToListAsync(cancellationToken);

        return Ok(pedidos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Pedido>> GetPedido(int id, CancellationToken cancellationToken)
    {
        var pedido = await GetPedidoQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.IdPedido == id, cancellationToken);

        return pedido is null ? NotFound() : Ok(pedido);
    }

    [HttpPost]
    public async Task<ActionResult<Pedido>> CrearPedido(PedidoRequest request, CancellationToken cancellationToken)
    {
        if (!await context.Clientes.AnyAsync(cliente => cliente.IdCliente == request.IdCliente, cancellationToken))
            return BadRequest(new { mensaje = "El cliente indicado no existe." });

        var pedido = new Pedido
        {
            IdCliente = request.IdCliente,
            Fecha = request.Fecha ?? DateTime.UtcNow,
            Estado = string.IsNullOrWhiteSpace(request.Estado) ? "Pendiente" : request.Estado.Trim(),
            Detalles = request.Detalles.Select(CrearDetalle).ToList()
        };
        pedido.Total = pedido.Detalles.Sum(detalle => detalle.Cantidad * detalle.PrecioUnitario);

        context.Pedidos.Add(pedido);
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Conflict(new { mensaje = "No se pudo guardar el pedido. Verifica sus datos y relaciones." });
        }

        var creado = await GetPedidoQuery().AsNoTracking()
            .FirstAsync(item => item.IdPedido == pedido.IdPedido, cancellationToken);
        return CreatedAtAction(nameof(GetPedido), new { id = pedido.IdPedido }, creado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> ActualizarPedido(int id, PedidoRequest request, CancellationToken cancellationToken)
    {
        var pedido = await context.Pedidos
            .Include(item => item.Detalles)
            .FirstOrDefaultAsync(item => item.IdPedido == id, cancellationToken);
        if (pedido is null) return NotFound();

        if (!await context.Clientes.AnyAsync(cliente => cliente.IdCliente == request.IdCliente, cancellationToken))
            return BadRequest(new { mensaje = "El cliente indicado no existe." });

        pedido.IdCliente = request.IdCliente;
        pedido.Fecha = request.Fecha ?? pedido.Fecha;
        pedido.Estado = string.IsNullOrWhiteSpace(request.Estado) ? pedido.Estado : request.Estado.Trim();

        context.DetallesPedido.RemoveRange(pedido.Detalles);
        pedido.Detalles = request.Detalles.Select(CrearDetalle).ToList();
        pedido.Total = pedido.Detalles.Sum(detalle => detalle.Cantidad * detalle.PrecioUnitario);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Conflict(new { mensaje = "No se pudo actualizar el pedido. Verifica sus datos y relaciones." });
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> EliminarPedido(int id, CancellationToken cancellationToken)
    {
        var pedido = await context.Pedidos
            .Include(item => item.Detalles)
            .FirstOrDefaultAsync(item => item.IdPedido == id, cancellationToken);
        if (pedido is null) return NotFound();

        context.DetallesPedido.RemoveRange(pedido.Detalles);
        context.Pedidos.Remove(pedido);
        await context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    private IQueryable<Pedido> GetPedidoQuery() => context.Pedidos
        .Include(pedido => pedido.Cliente)
        .Include(pedido => pedido.Detalles);

    private static DetallePedido CrearDetalle(DetallePedidoRequest detalle) => new()
    {
        IdProducto = detalle.IdProducto,
        Cantidad = detalle.Cantidad,
        PrecioUnitario = detalle.PrecioUnitario
    };
}
