using System.ComponentModel.DataAnnotations;

namespace Pedidos.Api.Dtos;

public sealed class PedidoRequest
{
    [Range(1, int.MaxValue)]
    public int IdCliente { get; set; }

    public DateTime? Fecha { get; set; }

    [StringLength(30)]
    public string? Estado { get; set; }

    [Required, MinLength(1)]
    public List<DetallePedidoRequest> Detalles { get; set; } = [];
}
