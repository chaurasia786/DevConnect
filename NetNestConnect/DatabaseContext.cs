using Microsoft.EntityFrameworkCore;
using NetNestConnect.Model;

namespace NetNestConnect
{
    public class DatabaseContext : DbContext 
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        { }
        public DbSet<UserRegistration> Registrations { get; set; }  
    }
}
