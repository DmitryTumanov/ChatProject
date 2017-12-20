using System.Data.Entity;

namespace ChatServer.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext():base("DefaultConnection")
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Account> Accounts { get; set; }
    }
}