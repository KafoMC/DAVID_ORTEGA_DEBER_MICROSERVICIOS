using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Inventario.Data.Entities;

public sealed class Existencia
{
    [Key] public int IdExistencia { get; set; }
    public int IdProducto { get; set; } // Referencia lógica al producto de Catalogo.Api; no hay FK entre microservicios.
    public int IdBodega { get; set; }
    [Range(0, int.MaxValue)] public int Stock { get; set; }
    public Bodega? Bodega { get; set; }
    [JsonIgnore] public List<MovimientoInventario> Movimientos { get; set; } = [];
}
