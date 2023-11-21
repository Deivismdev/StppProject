using System.Runtime.CompilerServices;
using API.Auth.Model;
using API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class GudAppContext : IdentityDbContext<User>
    {
        public GudAppContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}