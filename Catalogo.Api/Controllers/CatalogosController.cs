using Catalogo.Api.Data;using Catalogo.Api.Models;using Microsoft.AspNetCore.Mvc;using Microsoft.EntityFrameworkCore;
namespace Catalogo.Api.Controllers;
[ApiController,Route("api/[controller]")]public sealed class CategoriasController(CatalogoDbContext db):ControllerBase
{
 [HttpGet]public async Task<ActionResult<IEnumerable<Categoria>>> Get(CancellationToken ct)=>Ok(await db.Categorias.AsNoTracking().OrderBy(x=>x.Nombre).ToListAsync(ct));
 [HttpPost]public async Task<ActionResult<Categoria>> Post(Categoria x,CancellationToken ct){db.Categorias.Add(x);await db.SaveChangesAsync(ct);return CreatedAtAction(nameof(Get),new{id=x.IdCategoria},x);}
 [HttpPut("{id:int}")]public async Task<IActionResult> Put(int id,Categoria input,CancellationToken ct){var x=await db.Categorias.FindAsync([id],ct);if(x is null)return NotFound();x.Nombre=input.Nombre;await db.SaveChangesAsync(ct);return NoContent();}
 [HttpDelete("{id:int}")]public async Task<IActionResult> Delete(int id,CancellationToken ct){var x=await db.Categorias.FindAsync([id],ct);if(x is null)return NotFound();db.Categorias.Remove(x);try{await db.SaveChangesAsync(ct);}catch(DbUpdateException){return Conflict(new{mensaje="La categoría está asociada a productos."});}return NoContent();}
}
[ApiController,Route("api/[controller]")]public sealed class MarcasController(CatalogoDbContext db):ControllerBase
{
 [HttpGet]public async Task<ActionResult<IEnumerable<Marca>>> Get(CancellationToken ct)=>Ok(await db.Marcas.AsNoTracking().OrderBy(x=>x.Nombre).ToListAsync(ct));
 [HttpPost]public async Task<ActionResult<Marca>> Post(Marca x,CancellationToken ct){db.Marcas.Add(x);await db.SaveChangesAsync(ct);return CreatedAtAction(nameof(Get),new{id=x.IdMarca},x);}
 [HttpPut("{id:int}")]public async Task<IActionResult> Put(int id,Marca input,CancellationToken ct){var x=await db.Marcas.FindAsync([id],ct);if(x is null)return NotFound();x.Nombre=input.Nombre;await db.SaveChangesAsync(ct);return NoContent();}
 [HttpDelete("{id:int}")]public async Task<IActionResult> Delete(int id,CancellationToken ct){var x=await db.Marcas.FindAsync([id],ct);if(x is null)return NotFound();db.Marcas.Remove(x);try{await db.SaveChangesAsync(ct);}catch(DbUpdateException){return Conflict(new{mensaje="La marca está asociada a productos."});}return NoContent();}
}
