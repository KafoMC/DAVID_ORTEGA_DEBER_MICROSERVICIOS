using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Libro.Api.Models; 

namespace Libro.Api.Data
{
    public class LibroDBContext : DbContext
    {
        public LibroDBContext(DbContextOptions<LibroDBContext> options) : base(options)
        {
        }

        public DbSet<Libros> Libros { get; set; }
    }
}