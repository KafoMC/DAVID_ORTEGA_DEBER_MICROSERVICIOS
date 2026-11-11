using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedidos.Api.Data;
using Pedidos.Api.Dtos;
using Pedidos.Api.Models;

namespace Pedidos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ClientesController(PedidosDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes(CancellationToken cancellationToken) =>
        Ok(await context.Clientes.AsNoTracking().OrderBy(cliente => cliente.IdCliente).ToListAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Cliente>> GetCliente(int id, CancellationToken cancellationToken)
    {
        var cliente = await context.Clientes.AsNoTracking().FirstOrDefaultAsync(item => item.IdCliente == id, cancellationToken);
        return cliente is null ? NotFound() : Ok(cliente);
    }

    [HttpPost]
    public async Task<ActionResult<Cliente>> CrearCliente(ClienteRequest request, CancellationToken cancellationToken)
    {
        var cliente = new Cliente
        {
            Nombres = request.Nombres.Trim(),
            Cedula = request.Cedula?.Trim(),
            Telefono = request.Telefono?.Trim(),
            Correo = request.Correo?.Trim()
        };

        context.Clientes.Add(cliente);
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Conflict(new { mensaje = "No se pudo guardar el cliente. Comprueba si la cédula ya existe." });
        }

        return CreatedAtAction(nameof(GetCliente), new { id = cliente.IdCliente }, cliente);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> ActualizarCliente(int id, ClienteRequest request, CancellationToken cancellationToken)
    {
        var cliente = await context.Clientes.FirstOrDefaultAsync(item => item.IdCliente == id, cancellationToken);
        if (cliente is null) return NotFound();

        cliente.Nombres = request.Nombres.Trim();
        cliente.Cedula = request.Cedula?.Trim();
        cliente.Telefono = request.Telefono?.Trim();
        cliente.Correo = request.Correo?.Trim();

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Conflict(new { mensaje = "No se pudo actualizar el cliente. Comprueba si la cédula ya existe." });
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> EliminarCliente(int id, CancellationToken cancellationToken)
    {
        var cliente = await context.Clientes.FirstOrDefaultAsync(item => item.IdCliente == id, cancellationToken);
        if (cliente is null) return NotFound();

        context.Clientes.Remove(cliente);
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Conflict(new { mensaje = "No se puede eliminar el cliente porque tiene pedidos asociados." });
        }

        return NoContent();
    }
}
