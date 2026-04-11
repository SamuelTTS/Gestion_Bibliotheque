using Microsoft.EntityFrameworkCore;

namespace Gestion_Bibliotheque.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        // Cette ligne dit : "Je veux une table 'Livres' basée sur ma classe 'Livre'"
        public DbSet<Livres> Livres { get; set; }
        public DbSet<Users> Users { get; set; }
      
        }
}
