using System.Runtime.CompilerServices;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class GudAppContext : DbContext
    {
        public GudAppContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}