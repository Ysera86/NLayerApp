using Microsoft.AspNetCore.Diagnostics;
using NLayer.Core.DTOs;
using NLayer.Service.Exceptions;
using System.Text.Json;

namespace NLayer.API.Middlewares
{
    // middlewarerin "Use" ile başlası naming convention
    // extension için static olmak zorunda
    public static class UseCustomExceptionHandler
    {
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {
                // Run sonlandırıcı(terminal) middleware , yani request daha ileriye(actiona) gitmeden geri döner
                config.Run(async context =>
                {
                    // api olduğu için geriye json döncem, o nedene dönüş tipini belirttim
                    context.Response.ContentType = "application/json"; 
               
                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var statusCode = exceptionFeature.Error switch
                    {
                        // fırlatılan hata tipini ayırt etmeliyim, kendim mi fırlattım, sorun mu oldu, cliet kaynaklı mı
                        // clienttan mı kaynaklı olduğunu tanıyabilmek için de exception oluşturdum, onu forlatırım client hatalarında
                        ClientSideException => 400,
                        NotFoundException => 404,
                        _ => 500
                    };


                    context.Response.StatusCode = statusCode;
                    var response = CustomResponseDto<NoContentDto>.Fail(statusCode, exceptionFeature.Error.Message);

                    // controllerlarda .net normalde kendisi serialize ediyor otomatik ama Middlewarelerde responsu kendimiz serialize etmeliyiz
                    // JsonSerializer var .NET SDK içinde, Newtonsofta gerek yok
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                });
            });
        }
    }
}
