using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Libro.Api.Models
{
    [Table("Libros")]
    public class Libros
    {
        [Key]
        [Column("IdLibro")]
        public int IdLibro { get; set; }

        [StringLength(100)]
        [Column("Titulo", TypeName = "varchar(100)")]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(100)]
        [Column("Autor")]
        public string Autor { get; set; } = string.Empty;

        [StringLength(20)]
        [Column("Isbn")]
        public string Isbn { get; set; } = string.Empty;

        [Column("Precio", TypeName = "decimal(10, 2)")]
        public decimal Precio { get; set; }

        [Column("Estado")]
        public bool Estado { get; set; } = true;
    }
}