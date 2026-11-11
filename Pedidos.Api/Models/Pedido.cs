using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pedidos.Api.Models;

[Table("Pedido", Schema = "dbo")]
public sealed class Pedido
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdPedido { get; set; }

    public int IdCliente { get; set; }

    public DateTime? Fecha { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal Total { get; set; }

    [StringLength(30)]
    public string? Estado { get; set; }

    public Cliente? Cliente { get; set; }

    public List<DetallePedido> Detalles { get; set; } = [];
}
