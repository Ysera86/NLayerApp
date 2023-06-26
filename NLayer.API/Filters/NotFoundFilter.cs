using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Filters
{
    public class NotFoundFilter<T> : IAsyncActionFilter where T : BaseEntity
    {
        private readonly IService<T> _service;

        // Eğer bir filter ctorunda DI kullanıyorsa  :
        // 1 -  Program.cs e eklenmeli
        // 2 -  Action üzerinde [NotFoundFilter] ile attribute veremem,  [ServiceFilter(typeof(NotFoundFilter<Product>))] şeklinde tipini verip ServiceFilter ile kullanmam gerekli, bu nedenle Attribute classından miras almaya gerk yok, sadece IAsyncActionFilter yeterli
        public NotFoundFilter(IService<T> service)
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var idValue = context.ActionArguments.Values.FirstOrDefault();
            if (idValue == null)
            {
                await next.Invoke(); // yoluna devam et
                return;
            }

            var id = (int)idValue;
            var anyEntity = await _service.AnyAsync(x => x.Id == id);
            if (anyEntity)
            {
                await next.Invoke();
                return;
            }

            context.Result = new NotFoundObjectResult(CustomResponseDto<NoContentDto>.Fail(404, $"{typeof(T).Name} with Id {id} not found."));

        }
    }
}
