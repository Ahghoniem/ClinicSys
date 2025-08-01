using LibraryManagementSystem.DAL.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.RepositoriesContracts
{
    public interface IProductRepository :IGenericRepository<Product>
    {
    }
}
