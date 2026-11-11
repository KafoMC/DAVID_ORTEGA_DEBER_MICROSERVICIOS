using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Inventario.Data.Entities;

public sealed class MovimientoInventario
{
    [Key] public int IdMovimiento { get; set; }
    public int IdProducto { get; set; }
    public int IdBodega { get; set; }
    [Required, StringLength(20)] public string Tipo { get; set; } = "Entrada";
    [Range(1, int.MaxValue)] public int Cantidad { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    [JsonIgnore] public Existencia? Existencia { get; set; }
}
