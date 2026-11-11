using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Inventario.Data.Entities;

public sealed class ReservaStock
{
    [Key] public int IdReserva { get; set; }
    public int IdProducto { get; set; }
    public int IdBodega { get; set; }
    [Range(1, int.MaxValue)] public int Cantidad { get; set; }
    [Required, StringLength(20)] public string Estado { get; set; } = "Activa";
    [JsonIgnore] public Existencia? Existencia { get; set; }
}
