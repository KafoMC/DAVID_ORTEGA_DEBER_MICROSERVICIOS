using System.ComponentModel.DataAnnotations;

namespace Pedidos.Api.Dtos;

public sealed class DetallePedidoRequest
{
    [Range(1, int.MaxValue)]
    public int IdProducto { get; set; }

    [Range(1, int.MaxValue)]
    public int Cantidad { get; set; }

    [Range(typeof(decimal), "0", "99999999.99")]
    public decimal PrecioUnitario { get; set; }
}
