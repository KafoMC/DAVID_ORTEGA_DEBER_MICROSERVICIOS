using Catalogo.Api.Data;using Catalogo.Api.Models;using Microsoft.AspNetCore.Mvc;using Microsoft.EntityFrameworkCore;
namespace Catalogo.Api.Controllers;
[ApiController,Route("api/[controller]")] public sealed class ProductosController(CatalogoDbContext db):ControllerBase
{
 [HttpGet] public async Task<ActionResult<IEnumerable<Producto>>> Get(CancellationToken ct)=>Ok(await db.Productos.AsNoTracking().OrderBy(x=>x.Nombre).ToListAsync(ct));
 [HttpGet("{id:int}")] public async Task<ActionResult<Producto>> Get(int id,CancellationToken ct){var p=await db.Productos.AsNoTracking().FirstOrDefaultAsync(x=>x.IdProducto==id,ct);return p is null?NotFound(new{mensaje="Producto no encontrado"}):Ok(p);}
 [HttpPost] public async Task<ActionResult<Producto>> Post(Producto p,CancellationToken ct){if(!await db.Categorias.AnyAsync(x=>x.IdCategoria==p.IdCategoria,ct)||!await db.Marcas.AnyAsync(x=>x.IdMarca==p.IdMarca,ct))return BadRequest(new{mensaje="La categoría o marca indicada no existe."});db.Productos.Add(p);await db.SaveChangesAsync(ct);return CreatedAtAction(nameof(Get),new{id=p.IdProducto},p);}
 [HttpPut("{id:int}")] public async Task<IActionResult> Put(int id,Producto input,CancellationToken ct){var p=await db.Productos.FindAsync([id],ct);if(p is null)return NotFound();if(!await db.Categorias.AnyAsync(x=>x.IdCategoria==input.IdCategoria,ct)||!await db.Marcas.AnyAsync(x=>x.IdMarca==input.IdMarca,ct))return BadRequest(new{mensaje="La categoría o marca indicada no existe."});p.Nombre=input.Nombre;p.Descripcion=input.Descripcion;p.Precio=input.Precio;p.IdMarca=input.IdMarca;p.IdCategoria=input.IdCategoria;await db.SaveChangesAsync(ct);return NoContent();}
 [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id,CancellationToken ct){var p=await db.Productos.FindAsync([id],ct);if(p is null)return NotFound();db.Productos.Remove(p);await db.SaveChangesAsync(ct);return NoContent();}
}
