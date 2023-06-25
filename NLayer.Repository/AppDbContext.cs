using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using NLayer.Repository.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ilgili entity için  EntityConfiguration içine yazdık 
            //modelBuilder.Entity<Category>().HasKey(c => c.Id); 

            modelBuilder.Entity<ProductFeature>().HasData(
                new ProductFeature() { Id = 1, Color = "Red", Width = 10, Height = 20, ProductId = 1 },
                new ProductFeature() { Id = 2, Color = "Mavi", Width = 10, Height = 20, ProductId = 2 }
                  );


            //modelBuilder.ApplyConfiguration(new ProductConfiguration()); 
            // tek tek eklemeden 
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            base.OnModelCreating(modelBuilder);
        }
    }
}
