using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalogo.Api.Models;

public sealed class Producto
{
    [Key] public int IdProducto { get; set; }
    public int IdCategoria { get; set; }
    public int IdMarca { get; set; }
    [Required, StringLength(150)] public string Nombre { get; set; } = string.Empty;
    [Required, StringLength(40)] public string SKU { get; set; } = string.Empty;
    [Column(TypeName = "decimal(10,2)"), Range(typeof(decimal), "0", "99999999.99")] public decimal Precio { get; set; }
    [Required, StringLength(20)] public string Estado { get; set; } = "Activo";
    public Categoria? Categoria { get; set; }
    public Marca? Marca { get; set; }
}
