using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Configuration
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // bı 2si zaten efcore conventiona uyduğumuz için gerekmiyor bu projede
            //builder.HasKey(c => c.Id);
            //builder.Property(c => c.Id).UseIdentityColumn();

            /// default olarak  public DbSet<Category> Categories { get; set; } buradan Categories alır.
            //builder.ToTable("Categories");


            builder.Property(c => c.Name).IsRequired().HasMaxLength(50);
        }
    }
}
