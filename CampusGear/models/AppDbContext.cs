using Microsoft.EntityFrameworkCore;

namespace CampusGear.models
{
    public class AppDbContext : DbContext 
    {
       
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {

        }
        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        

    }

    }
