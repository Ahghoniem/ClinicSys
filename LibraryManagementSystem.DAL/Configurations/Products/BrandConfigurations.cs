using LibraryManagementSystem.DAL.Configurations;
using LibraryManagementSystem.DAL.Entities.Products;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.Configurations.Products
{
    internal class BrandConfigurations : BaseEntityConfigurations<ProductBrand>
    {
        public override void Configure(EntityTypeBuilder<ProductBrand> builder)
        {
            base.Configure(builder);

            builder.Property(b => b.Name).IsRequired();
        }
    }
}
