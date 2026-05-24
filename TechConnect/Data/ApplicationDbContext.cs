using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechConnect.Models;

namespace TechConnect.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TechConnect.Models.Categoria> Categoria { get; set; } = default!;
        public DbSet<TechConnect.Models.Contato> Contato { get; set; } = default!;
        public DbSet<TechConnect.Models.Evento> Evento { get; set; } = default!;
        public DbSet<TechConnect.Models.Palestrante> Palestrante { get; set; } = default!;
    }
}
