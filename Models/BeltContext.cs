using Microsoft.EntityFrameworkCore;
 
namespace beltexam.Models
{
    public class BeltContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public DbSet<User> Users  { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Rsvp> Rsvps { get; set; }
        public BeltContext(DbContextOptions<BeltContext> options) : base(options) {}
            
        }
    }

