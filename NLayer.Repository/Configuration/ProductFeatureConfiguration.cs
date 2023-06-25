using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Configuration
{
    internal class ProductFeatureConfiguration : IEntityTypeConfiguration<ProductFeature>
    {
        public void Configure(EntityTypeBuilder<ProductFeature> builder)
        {
            // zaten efcore conventiona uyduğumuz için gerekmiyor bu projede
            //builder.HasOne(p => p.Product).WithOne(x => x.ProductFeature).HasForeignKey<ProductFeature>(x => x.ProductId;
          
        }
    }
}
