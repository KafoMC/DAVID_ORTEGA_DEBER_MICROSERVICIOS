using Inventario.Data;
using Inventario.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Controllers;

[ApiController, Route("api/[controller]")]
public sealed class MovimientosController(InventarioDBContext db) : ControllerBase
{
    [HttpGet] public async Task<ActionResult<IEnumerable<MovimientoInventario>>> Get(CancellationToken ct) => Ok(await db.MovimientosInventario.AsNoTracking().OrderByDescending(x => x.Fecha).ToListAsync(ct));
    [HttpPost] public async Task<ActionResult<MovimientoInventario>> Post(MovimientoInventario input, CancellationToken ct)
    {
        if (input.Cantidad <= 0 || input.Tipo is not ("Entrada" or "Salida")) return BadRequest(new { mensaje = "Tipo debe ser Entrada o Salida y cantidad mayor que cero." });
        var stock = await db.Existencias.FirstOrDefaultAsync(x => x.IdProducto == input.IdProducto && x.IdBodega == input.IdBodega, ct);
        if (stock is null) return NotFound(new { mensaje = "No existe stock registrado para ese producto y bodega." });
        if (input.Tipo == "Salida")
        {
            var reservado = await db.ReservasStock.Where(x => x.IdProducto == input.IdProducto && x.IdBodega == input.IdBodega && x.Estado == "Activa").SumAsync(x => (int?)x.Cantidad, ct) ?? 0;
            if (stock.Stock - reservado < input.Cantidad) return Conflict(new { mensaje = "Stock disponible insuficiente (se excluyen las reservas activas)." });
            stock.Stock -= input.Cantidad;
        }
        else stock.Stock += input.Cantidad;
        input.Fecha = input.Fecha == default ? DateTime.UtcNow : input.Fecha;
        db.MovimientosInventario.Add(input);
        await db.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(Get), new { id = input.IdMovimiento }, input);
    }
}
