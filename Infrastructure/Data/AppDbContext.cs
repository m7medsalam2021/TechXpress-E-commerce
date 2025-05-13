using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; } // Cart table
        public DbSet<Address> Addresses { get; set; } // Address table
        public DbSet<Order> Orders { get; set; } // Order table
        public DbSet<OrderProduct> OrderProducts { get; set; } // OrderProduct table
        public DbSet<Category> Categories { get; set; } // Wishlist table
        public DbSet<SliderImage> SliderImageS { get; set; } // SliderImageS table
        public DbSet<User> Users { get; set; } // User table
    }
}
