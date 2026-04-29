using Microsoft.EntityFrameworkCore;
using CinemaApi.Models;

namespace CinemaApi.Data;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
    }
