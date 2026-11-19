using Microsoft.EntityFrameworkCore;

namespace CampusGear.Api.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Equipment> Equipments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Laptops" },
                new Category { CategoryId = 2, CategoryName = "Cameras" },
                new Category { CategoryId = 3, CategoryName = "Projectors" });

            modelBuilder.Entity<Equipment>().HasData(
                new Equipment { EquipmentId = 1, EquipmentName = "Dell Latitude 5440", CategoryId = 1, Description = "14-inch laptop", Location = "Library Desk A", Condition = "Good", IsAvailable = true },
                new Equipment { EquipmentId = 2, EquipmentName = "MacBook Air M2", CategoryId = 1, Description = "13-inch laptop", Location = "Library Desk A", Condition = "Excellent", IsAvailable = true },
                new Equipment { EquipmentId = 3, EquipmentName = "Canon EOS R50", CategoryId = 2, Description = "Mirrorless camera", Location = "Media Lab", Condition = "Good", IsAvailable = true },
                new Equipment { EquipmentId = 4, EquipmentName = "Sony A6000", CategoryId = 2, Description = "Mirrorless camera", Location = "Media Lab", Condition = "In repair", IsAvailable = false },
                new Equipment { EquipmentId = 5, EquipmentName = "Epson Projector", CategoryId = 3, Description = "1080p projector", Location = "Room B204", Condition = "Good", IsAvailable = true });

            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, FirstName = "Test", LastName = "Student", Email = "student@campus.ca", StudentNumber = "991000001" },
                new User { UserId = 2, FirstName = "Second", LastName = "Student", Email = "student2@campus.ca", StudentNumber = "991000002" });
        }
    }
}
