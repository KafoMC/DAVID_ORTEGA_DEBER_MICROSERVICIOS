using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Catalogo.Api.Models;

public sealed class Categoria
{
    [Key] public int IdCategoria { get; set; }
    [Required, StringLength(100)] public string Nombre { get; set; } = string.Empty;
    [StringLength(300)] public string? Descripcion { get; set; }
    [JsonIgnore] public List<Producto> Productos { get; set; } = [];
}
