using Microsoft.AspNetCore.Http;

using System.Reflection;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Infrastructure.Middlewares
{
    public class VersionMiddleware
    {
        public VersionMiddleware(RequestDelegate next)
        {
        }

        public async Task InvokeAsync(HttpContext context)
        {
            AssemblyName assembly = Assembly.GetExecutingAssembly().GetName();
            var response = new
            {
                version = assembly.Version?.ToString() ?? "no version",
                serviceName = assembly.Name,
            };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}