using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
            //modelBuilder.ApplyConfiguration(new ProductConfiguration()); 
            // tek tek eklemeden 
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            // ilgili entity için  EntityConfiguration içine yazdık 
            //modelBuilder.Entity<Category>().HasKey(c => c.Id); 

            modelBuilder.Entity<ProductFeature>().HasData(
                new ProductFeature() { Id = 1, Color = "Red", Width = 10, Height = 20, ProductId = 1 },
                new ProductFeature() { Id = 2, Color = "Mavi", Width = 10, Height = 20, ProductId = 2 }
                  );


            base.OnModelCreating(modelBuilder);
        }


        public override int SaveChanges()
        {
            UpdateChangeTracker();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateChangeTracker();
            return base.SaveChangesAsync(cancellationToken);
        }

        public void UpdateChangeTracker()
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                Entry(entityReference).Property(x => x.UpdatedDate).IsModified = false;
                                entityReference.CreatedDate = DateTime.Now;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;
                                entityReference.UpdatedDate = DateTime.Now;
                                break;
                            }
                    }

                }
            }
        }
    }
}
