using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Inventario.Data.Entities;

public sealed class Bodega
{
    [Key] public int IdBodega { get; set; }
    [Required, StringLength(100)] public string Nombre { get; set; } = string.Empty;
    [Required, StringLength(100)] public string Ciudad { get; set; } = string.Empty;
    [Required, StringLength(250)] public string Direccion { get; set; } = string.Empty;
    [JsonIgnore] public List<Existencia> Existencias { get; set; } = [];
}
