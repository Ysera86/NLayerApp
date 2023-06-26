using NLayer.Core.DTOs;
using NLayer.Core.Models;
using System.Linq.Expressions;

namespace NLayer.Core.Services
{
    public interface IProductService : IService<Product>
    {
        //Task<List<ProductWithCategoryDto>> GetProductsWithCategory();
        Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategoryAsync();
    }
}
