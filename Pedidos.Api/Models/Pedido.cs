using System.ComponentModel.DataAnnotations;using System.Text.Json.Serialization;
namespace Pedidos.Api.Models;
public sealed class Cliente{public int IdCliente{get;set;}[Required,StringLength(120)]public string Nombre{get;set;}="";public string? Cedula{get;set;}public string? Telefono{get;set;}public string? Correo{get;set;}[JsonIgnore]public List<Pedido> Pedidos{get;set;}=[];}
public sealed class Pedido{public int IdPedido{get;set;}public int IdCliente{get;set;}public DateTime? Fecha{get;set;}=DateTime.UtcNow;public string? Estado{get;set;}="Confirmado";public Cliente? Cliente{get;set;}public List<DetallePedido> Detalles{get;set;}=[];}
public sealed class DetallePedido{public int IdDetallePedido{get;set;}public int IdPedido{get;set;}public int IdProducto{get;set;}public int IdBodega{get;set;}[Range(1,int.MaxValue)]public int Cantidad{get;set;}public decimal PrecioUnitario{get;set;}[JsonIgnore]public Pedido? Pedido{get;set;}}
