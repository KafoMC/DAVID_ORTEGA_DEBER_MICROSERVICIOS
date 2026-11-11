using Libro.Api.Data;
using Libro.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Net.Http; // Necesario para IHttpClientFactory y HttpRequestException
using System.Threading.Tasks; // Necesario para TaskCanceledException
using System; // Necesario para Exception

namespace Libro.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly LibroDBContext _context;
        private readonly IHttpClientFactory _httpClientFactory; // Campo añadido

        // Se inyecta IHttpClientFactory en el constructor
        public LibrosController(LibroDBContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libros>>> GetLibros() // Programacion asincrona
        {
            var libros = await _context.Libros
                .AsNoTracking()
                .ToListAsync();

            return Ok(libros);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Libros>> GetLibro(int id)
        {
            var libro = await _context.Libros
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.IdLibro == id);

            if (libro == null) return NotFound(new { mensaje = "Libro no encontrado" });

            try
            {
                // Se crea el cliente
                var clienteApi = _httpClientFactory.CreateClient("ClienteApi");

              
            }
            catch (TaskCanceledException)
            {
                return StatusCode(504, new { mensaje = "Tiempo de espera agotado al intentar obtener el cliente." });
            }
            catch (HttpRequestException)
            {
               
                return StatusCode(502, new { mensaje = "Error de red al intentar obtener el cliente." });
            }
            catch (JsonException)
            {
                return StatusCode(500, new { mensaje = "Error al procesar el JSON de la respuesta del cliente." });
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new { mensaje = "Error al intentar obtener el cliente.", detalle = ex.Message });
            }

            return Ok(libro);
        }

        [HttpPost]
        public async Task<ActionResult<Libros>> CrearLibro(Libros libro)
        {
            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();

            // Retorna un código 201 Created con la URL para obtener el nuevo recurso
            return CreatedAtAction(nameof(GetLibro), new { id = libro.IdLibro }, libro);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ActualizarLibro(int id, Libros libro)
        {
            if (id != libro.IdLibro) return BadRequest(new { mensaje = "El ID del parámetro no coincide con el ID del modelo." });

            _context.Entry(libro).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent(); // Estado 204: No Content
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarLibro(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null) return NotFound();

            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}