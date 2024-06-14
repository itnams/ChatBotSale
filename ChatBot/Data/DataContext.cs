using ChatBot.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}
        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<Product> Product { get; set; }

    }
}
