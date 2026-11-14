using System.ComponentModel.DataAnnotations;
namespace Catalogo.Api.Models;
public sealed class Producto { public int IdProducto {get;set;} [Required,StringLength(120)] public string Nombre {get;set;}=""; [StringLength(250)] public string? Descripcion {get;set;} [Range(typeof(decimal),"0","99999999")] public decimal Precio {get;set;} public int IdMarca {get;set;} public int IdCategoria {get;set;} }
public sealed class Categoria { public int IdCategoria {get;set;} public string Nombre {get;set;}=""; }
public sealed class Marca { public int IdMarca {get;set;} public string Nombre {get;set;}=""; }
