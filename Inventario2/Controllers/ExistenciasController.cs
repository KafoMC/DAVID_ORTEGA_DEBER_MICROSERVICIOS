using Inventario.Data;
using Inventario.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Controllers;

[ApiController, Route("api/[controller]")]
public sealed class ExistenciasController(InventarioDBContext db) : ControllerBase
{
    [HttpGet] public async Task<ActionResult<IEnumerable<Existencia>>> Get(CancellationToken ct) => Ok(await db.Existencias.AsNoTracking().Include(x => x.Bodega).OrderBy(x => x.IdProducto).ThenBy(x => x.IdBodega).ToListAsync(ct));
    [HttpGet("{idProducto:int}/{idBodega:int}")] public async Task<ActionResult<Existencia>> Get(int idProducto, int idBodega, CancellationToken ct) { var x = await db.Existencias.AsNoTracking().Include(x => x.Bodega).FirstOrDefaultAsync(x => x.IdProducto == idProducto && x.IdBodega == idBodega, ct); return x is null ? NotFound() : Ok(x); }
    [HttpPost] public async Task<ActionResult<Existencia>> Post(Existencia x, CancellationToken ct) { if (!await db.Bodegas.AnyAsync(b => b.IdBodega == x.IdBodega, ct)) return BadRequest(new { mensaje = "La bodega no existe." }); db.Existencias.Add(x); try { await db.SaveChangesAsync(ct); } catch (DbUpdateException) { return Conflict(new { mensaje = "Ya existe una existencia para este producto y bodega." }); } return CreatedAtAction(nameof(Get), new { idProducto = x.IdProducto, idBodega = x.IdBodega }, x); }
    [HttpPut("{idProducto:int}/{idBodega:int}")] public async Task<IActionResult> Put(int idProducto, int idBodega, Existencia input, CancellationToken ct) { var x = await db.Existencias.FirstOrDefaultAsync(x => x.IdProducto == idProducto && x.IdBodega == idBodega, ct); if (x is null) return NotFound(); if (input.Stock < 0) return BadRequest(); x.Stock = input.Stock; await db.SaveChangesAsync(ct); return NoContent(); }
}
