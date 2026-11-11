using Catalogo.Api.Data;
using Catalogo.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Api.Controllers;

[ApiController, Route("api/[controller]")]
public sealed class MarcasController(CatalogoDbContext db) : ControllerBase
{
    [HttpGet] public async Task<ActionResult<IEnumerable<Marca>>> Get(CancellationToken ct) => Ok(await db.Marcas.AsNoTracking().OrderBy(x => x.Nombre).ToListAsync(ct));
    [HttpGet("{id:int}")] public async Task<ActionResult<Marca>> Get(int id, CancellationToken ct) { var item = await db.Marcas.AsNoTracking().FirstOrDefaultAsync(x => x.IdMarca == id, ct); return item is null ? NotFound() : Ok(item); }
    [HttpPost] public async Task<ActionResult<Marca>> Post(Marca item, CancellationToken ct) { db.Marcas.Add(item); await db.SaveChangesAsync(ct); return CreatedAtAction(nameof(Get), new { id = item.IdMarca }, item); }
    [HttpPut("{id:int}")] public async Task<IActionResult> Put(int id, Marca input, CancellationToken ct) { var item = await db.Marcas.FindAsync([id], ct); if (item is null) return NotFound(); item.Nombre = input.Nombre; item.PaisOrigen = input.PaisOrigen; await db.SaveChangesAsync(ct); return NoContent(); }
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id, CancellationToken ct) { var item = await db.Marcas.FindAsync([id], ct); if (item is null) return NotFound(); db.Marcas.Remove(item); try { await db.SaveChangesAsync(ct); } catch (DbUpdateException) { return Conflict(new { mensaje = "La marca tiene productos asociados." }); } return NoContent(); }
}
