using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;

namespace NLayer.API.Controllers
{
    [Route("api/[controller]")]
    //[Route("api/[controller]/[action]")] > olsaydı istek yaparken methodun ismini yazmak gerekirdi, bu şekilde olmadığından http methoda göre istek yaparken methodun tipine göre eşleştiriyor *
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        [NonAction] // bu bir endpoint değil, swagger bunu öyle algılar eklemezsek ve Http methodu olmadığından da hata fırlatır.
        public IActionResult CreateActionResult<T>(CustomResponseDto<T> response)
        {
            if (response.StatusCode == 204) {
                return new ObjectResult(null)
                {
                    StatusCode = response.StatusCode
                };
            }

            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
