using Microsoft.AspNetCore.Http;

using System.Net;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Infrastructure.Middlewares
{
    public class ReadyMiddleware
    {
        public ReadyMiddleware(RequestDelegate next)
        {
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;
        }
    }
}