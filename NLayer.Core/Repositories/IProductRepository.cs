using NLayer.Core.Models;
using System.Linq.Expressions;

namespace NLayer.Core.Repositories
{
    public interface IProductRepository : IGenericRepository<Product> 
    {       
        Task<List<Product>> GetProductsWithCategoryAsync(); 
    }
}
