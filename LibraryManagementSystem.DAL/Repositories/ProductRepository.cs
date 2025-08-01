using LibraryManagementSystem.DAL.DbContext;
using LibraryManagementSystem.DAL.Entities.Products;
using LibraryManagementSystem.DAL.RepositoriesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(LibraryDbContext dbContext) : base(dbContext)
        {
        }
    }
}
