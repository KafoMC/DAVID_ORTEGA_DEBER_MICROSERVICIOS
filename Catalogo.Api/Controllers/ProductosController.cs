using Catalogo.Api.Data;
using Catalogo.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Api.Controllers;

[ApiController, Route("api/[controller]")]
public sealed class ProductosController(CatalogoDbContext db) : ControllerBase
{
    private IQueryable<Producto> Consulta() => db.Productos.AsNoTracking().Include(x => x.Categoria).Include(x => x.Marca);
    [HttpGet] public async Task<ActionResult<IEnumerable<Producto>>> Get(CancellationToken ct) => Ok(await Consulta().OrderBy(x => x.Nombre).ToListAsync(ct));
    [HttpGet("{id:int}")] public async Task<ActionResult<Producto>> Get(int id, CancellationToken ct) { var item = await Consulta().FirstOrDefaultAsync(x => x.IdProducto == id, ct); return item is null ? NotFound() : Ok(item); }
    [HttpPost] public async Task<ActionResult<Producto>> Post(Producto item, CancellationToken ct) { if (!await db.Categorias.AnyAsync(x => x.IdCategoria == item.IdCategoria, ct) || !await db.Marcas.AnyAsync(x => x.IdMarca == item.IdMarca, ct)) return BadRequest(new { mensaje = "La categoría y la marca deben existir." }); db.Productos.Add(item); try { await db.SaveChangesAsync(ct); } catch (DbUpdateException) { return Conflict(new { mensaje = "El SKU ya está registrado." }); } return CreatedAtAction(nameof(Get), new { id = item.IdProducto }, item); }
    [HttpPut("{id:int}")] public async Task<IActionResult> Put(int id, Producto input, CancellationToken ct) { var item = await db.Productos.FirstOrDefaultAsync(x => x.IdProducto == id, ct); if (item is null) return NotFound(); if (!await db.Categorias.AnyAsync(x => x.IdCategoria == input.IdCategoria, ct) || !await db.Marcas.AnyAsync(x => x.IdMarca == input.IdMarca, ct)) return BadRequest(new { mensaje = "La categoría y la marca deben existir." }); item.IdCategoria = input.IdCategoria; item.IdMarca = input.IdMarca; item.Nombre = input.Nombre; item.SKU = input.SKU; item.Precio = input.Precio; item.Estado = input.Estado; try { await db.SaveChangesAsync(ct); } catch (DbUpdateException) { return Conflict(new { mensaje = "El SKU ya está registrado." }); } return NoContent(); }
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id, CancellationToken ct) { var item = await db.Productos.FindAsync([id], ct); if (item is null) return NotFound(); db.Productos.Remove(item); await db.SaveChangesAsync(ct); return NoContent(); }
}
