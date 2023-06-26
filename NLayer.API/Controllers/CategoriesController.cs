using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    // CustomBaseController içinden geliyorlar
    //[Route("api/[controller]")]
    //[ApiController]
    public class CategoriesController : CustomBaseController
    {
        private readonly ICategoryService _categoryService;
        //private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetCategoryByIdWithProducts(int id)
        {
            return CreateActionResult(await _categoryService.GetCategoryByIdWithProductsAsync(id)); 
        }
    }
}
