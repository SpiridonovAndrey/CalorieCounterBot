using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CalorieCounterBot
{
    internal class Connection
    {
    }
    public class ApplicationContext : DbContext
    {
        public DbSet<Users> Users { get; set; } = null!;
        public DbSet<Products> Products { get; set; } = null!;
        public DbSet<ProductCalories> ProductCalories { get; set; } = null!;
        public DbSet<UserProduct> UserProduct { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=CalorieCounter;persist security info=True;TrustServerCertificate=True;Trusted_Connection=True;");
        }
    }
}
