using GoodsStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GoodsStore.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<DeliveryQueue> DeliveryQueues{ get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Products> Products { get; set; }
    }
}
