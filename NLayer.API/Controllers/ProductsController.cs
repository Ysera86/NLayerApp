using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    public class ProductsController : CustomBaseController
    {
        private readonly IMapper _mapper;
        //private readonly IService<Product> _service;
        private readonly IProductService _service;

        //public ProductsController(IService<Product> service, IMapper mapper)
        public ProductsController(IProductService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // * GET api/products/GetProductsWithCategory çnk 1den fazla GET methodlu endpoint oldu artık
        // [HttpGet("[action]")] // otomatik method adını alır bu şekilde belirtirsek.
        // Yani burada [HttpGet("[action]")] = [HttpGet("GetProductsWithCategory")]
        [HttpGet("GetProductsWithCategory")]
        public async Task<IActionResult> GetProductsWithCategory() 
        {
            return CreateActionResult(await _service.GetProductsWithCategoryAsync());
        }

        // * GET api/products ise bu method
        [HttpGet] 
        public async Task<IActionResult> All()
        {
            var products = await _service.GetAllAsync();
            var productsDto = _mapper.Map<List<ProductDto>>(products.ToList());
            //return Ok(CustomResponseDto<List<ProductDto>>.Success(200, productsDto));
            return CreateActionResult(CustomResponseDto<List<ProductDto>>.Success(200, productsDto));
        }

        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        // * GET api/products/5 ise bu method
        [HttpGet("{id}")] // www.abc.com/products/5   
        public async Task<IActionResult> GetById(int id) //  [HttpGet("{id}")] şeklinde http methodda belirmeseydik bu id'yi querystringtem beklerdi. > // www.abc.com/products?id=5   
        {
            var product = await _service.GetByIdAsync(id);
            var productDto = _mapper.Map<ProductDto>(product);
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(200, productDto));
        }
                
        [HttpPost()] 
        public async Task<IActionResult> Save(ProductDto productDto) 
        {
            var addedProduct =  await _service.AddAsync(_mapper.Map<Product>(productDto));
            var addedProductDto = _mapper.Map<ProductDto>(addedProduct);
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(201, addedProductDto));
        }
                
        [HttpPut()] 
        public async Task<IActionResult> Update(ProductUpdateDto productUpdateDto) 
        {
            await _service.UpdateAsync(_mapper.Map<Product>(productUpdateDto));
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        // * DELETE api/products/5 ise bu method
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var product = await _service.GetByIdAsync(id);

            // bu ve benzeri kodları 1000 yerde yapmamalıyız, controllerlar temiz kalmadı. mrekezi bir yere eklemeliyiz.
            //if (product == null)
            //{
            //    return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(404,"bu id'ye sahip ürün bulunamadı"));
            //}

            await _service.RemoveAsync(product);
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
    }
}
