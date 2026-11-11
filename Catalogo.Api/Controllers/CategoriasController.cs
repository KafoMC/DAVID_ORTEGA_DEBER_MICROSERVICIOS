using Catalogo.Api.Data;
using Catalogo.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Api.Controllers;

[ApiController, Route("api/[controller]")]
public sealed class CategoriasController(CatalogoDbContext db) : ControllerBase
{
    [HttpGet] public async Task<ActionResult<IEnumerable<Categoria>>> Get(CancellationToken ct) => Ok(await db.Categorias.AsNoTracking().OrderBy(x => x.Nombre).ToListAsync(ct));
    [HttpGet("{id:int}")] public async Task<ActionResult<Categoria>> Get(int id, CancellationToken ct) { var item = await db.Categorias.AsNoTracking().FirstOrDefaultAsync(x => x.IdCategoria == id, ct); return item is null ? NotFound() : Ok(item); }
    [HttpPost] public async Task<ActionResult<Categoria>> Post(Categoria item, CancellationToken ct) { db.Categorias.Add(item); await db.SaveChangesAsync(ct); return CreatedAtAction(nameof(Get), new { id = item.IdCategoria }, item); }
    [HttpPut("{id:int}")] public async Task<IActionResult> Put(int id, Categoria input, CancellationToken ct) { var item = await db.Categorias.FindAsync([id], ct); if (item is null) return NotFound(); item.Nombre = input.Nombre; item.Descripcion = input.Descripcion; await db.SaveChangesAsync(ct); return NoContent(); }
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id, CancellationToken ct) { var item = await db.Categorias.FindAsync([id], ct); if (item is null) return NotFound(); db.Categorias.Remove(item); try { await db.SaveChangesAsync(ct); } catch (DbUpdateException) { return Conflict(new { mensaje = "La categoría tiene productos asociados." }); } return NoContent(); }
}
