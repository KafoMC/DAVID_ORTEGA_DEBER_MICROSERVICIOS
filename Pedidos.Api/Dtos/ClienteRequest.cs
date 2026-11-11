using System.ComponentModel.DataAnnotations;

namespace Pedidos.Api.Dtos;

public sealed class ClienteRequest
{
    [Required, StringLength(150)]
    public string Nombres { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Cedula { get; set; }

    [StringLength(20)]
    public string? Telefono { get; set; }

    [StringLength(100), EmailAddress]
    public string? Correo { get; set; }
}
