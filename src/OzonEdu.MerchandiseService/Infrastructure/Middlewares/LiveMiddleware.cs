using Microsoft.AspNetCore.Http;

using System.Net;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Infrastructure.Middlewares
{
    public class LiveMiddleware
    {
        public LiveMiddleware(RequestDelegate next)
        {
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;
        }
    }
}