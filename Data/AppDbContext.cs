using GoodsStore.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodsStore.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<DeliveryQueue> DeliveryQueues{ get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Products> Products { get; set; }
    }
}
