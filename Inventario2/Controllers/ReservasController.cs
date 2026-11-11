using Inventario.Data;
using Inventario.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Controllers;

[ApiController, Route("api/[controller]")]
public sealed class ReservasController(InventarioDBContext db) : ControllerBase
{
    [HttpGet] public async Task<ActionResult<IEnumerable<ReservaStock>>> Get(CancellationToken ct) => Ok(await db.ReservasStock.AsNoTracking().OrderByDescending(x => x.IdReserva).ToListAsync(ct));
    [HttpPost] public async Task<ActionResult<ReservaStock>> Post(ReservaStock input, CancellationToken ct)
    {
        if (input.Cantidad <= 0) return BadRequest(new { mensaje = "La cantidad debe ser mayor que cero." });
        var stock = await db.Existencias.FirstOrDefaultAsync(x => x.IdProducto == input.IdProducto && x.IdBodega == input.IdBodega, ct);
        if (stock is null) return NotFound(new { mensaje = "No existe stock registrado para ese producto y bodega." });
        var reservado = await db.ReservasStock.Where(x => x.IdProducto == input.IdProducto && x.IdBodega == input.IdBodega && x.Estado == "Activa").SumAsync(x => (int?)x.Cantidad, ct) ?? 0;
        if (stock.Stock - reservado < input.Cantidad) return Conflict(new { mensaje = "Stock disponible insuficiente." });
        input.Estado = "Activa"; db.ReservasStock.Add(input); await db.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(Get), new { id = input.IdReserva }, input);
    }
    [HttpPatch("{id:int}/liberar")] public async Task<IActionResult> Liberar(int id, CancellationToken ct) { var r = await db.ReservasStock.FirstOrDefaultAsync(x => x.IdReserva == id, ct); if (r is null) return NotFound(); if (r.Estado != "Activa") return Conflict(new { mensaje = "Solo se pueden liberar reservas activas." }); r.Estado = "Liberada"; await db.SaveChangesAsync(ct); return NoContent(); }
}
