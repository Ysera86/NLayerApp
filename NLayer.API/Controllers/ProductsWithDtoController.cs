using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    public class ProductsWithDtoController : CustomBaseController
    {
        private readonly IProductServiceWithDto _service;

        public ProductsWithDtoController(IProductServiceWithDto service)
        {
            _service = service;
        }

       
        [HttpGet("GetProductsWithCategory")]
        public async Task<IActionResult> GetProductsWithCategory() 
        {
            return CreateActionResult(await _service.GetProductsWithCategoryAsync());
        }

        [HttpGet] 
        public async Task<IActionResult> All()
        {
            return CreateActionResult(await _service.GetAllAsync());
        }

        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpGet("{id}")] 
        public async Task<IActionResult> GetById(int id) 
        {
            return CreateActionResult(await _service.GetByIdAsync(id));
        }
                
        [HttpPost()] 
        public async Task<IActionResult> Save(ProductCreateDto productDto) 
        {
            return CreateActionResult(await _service.AddAsync(productDto));
        }
                
        [HttpPut()] 
        public async Task<IActionResult> Update(ProductUpdateDto productUpdateDto) 
        {
            return CreateActionResult(await _service.UpdateAsync(productUpdateDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            return CreateActionResult(await _service.RemoveAsync(id));
        }

        [HttpPost("SaveRange")]
        public async Task<IActionResult> SaveRange(List<ProductDto> products)
        {
            return CreateActionResult(await _service.AddRangeAsync(products));
        }

        [HttpPost("RemoveRange")]
        public async Task<IActionResult> RemoveRange(List<int> ids)
        {
            return CreateActionResult(await _service.RemoveRangeAsync(ids));
        }

        [HttpGet("Any")]
        public async Task<IActionResult> Any(int id)
        {
            return CreateActionResult(await _service.AnyAsync(x => x.Id == id));
        }
    }
}
