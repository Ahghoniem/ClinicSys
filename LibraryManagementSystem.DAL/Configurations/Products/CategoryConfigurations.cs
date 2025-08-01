using LibraryManagementSystem.DAL.Configurations;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.Configurations.Products
{
    internal class CategoryConfigurations : BaseEntityConfigurations<ProductCategory>
    {
        public override void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            base.Configure(builder);
            builder.Property(C => C.Name).IsRequired();
        }
    }
}
