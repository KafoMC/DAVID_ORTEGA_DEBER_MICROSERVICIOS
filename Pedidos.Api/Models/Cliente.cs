using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pedidos.Api.Models;

[Table("Cliente", Schema = "dbo")]
public sealed class Cliente
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdCliente { get; set; }

    [Required, StringLength(150)]
    public string Nombres { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Cedula { get; set; }

    [StringLength(20)]
    public string? Telefono { get; set; }

    [StringLength(100), EmailAddress]
    public string? Correo { get; set; }
}
