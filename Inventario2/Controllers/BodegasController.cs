using Inventario.Data;
using Inventario.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Controllers;

[ApiController, Route("api/[controller]")]
public sealed class BodegasController(InventarioDBContext db) : ControllerBase
{
    [HttpGet] public async Task<ActionResult<IEnumerable<Bodega>>> Get(CancellationToken ct) => Ok(await db.Bodegas.AsNoTracking().OrderBy(x => x.Nombre).ToListAsync(ct));
    [HttpGet("{id:int}")] public async Task<ActionResult<Bodega>> Get(int id, CancellationToken ct) { var x = await db.Bodegas.AsNoTracking().FirstOrDefaultAsync(x => x.IdBodega == id, ct); return x is null ? NotFound() : Ok(x); }
    [HttpPost] public async Task<ActionResult<Bodega>> Post(Bodega x, CancellationToken ct) { db.Bodegas.Add(x); await db.SaveChangesAsync(ct); return CreatedAtAction(nameof(Get), new { id = x.IdBodega }, x); }
    [HttpPut("{id:int}")] public async Task<IActionResult> Put(int id, Bodega input, CancellationToken ct) { var x = await db.Bodegas.FindAsync([id], ct); if (x is null) return NotFound(); x.Nombre = input.Nombre; x.Ciudad = input.Ciudad; x.Direccion = input.Direccion; await db.SaveChangesAsync(ct); return NoContent(); }
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id, CancellationToken ct) { var x = await db.Bodegas.FindAsync([id], ct); if (x is null) return NotFound(); db.Bodegas.Remove(x); try { await db.SaveChangesAsync(ct); } catch (DbUpdateException) { return Conflict(new { mensaje = "La bodega tiene existencias asociadas." }); } return NoContent(); }
}
