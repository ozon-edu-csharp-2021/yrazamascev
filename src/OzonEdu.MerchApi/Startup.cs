using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

using OzonEdu.MerchApi.GrpcServices;
using OzonEdu.MerchApi.Services;
using OzonEdu.MerchApi.Services.Interfaces;

namespace OzonEdu.MerchApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMerchService, MerchService>();
            services.AddGrpc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<MerchApiGrpsService>();
                endpoints.MapControllers();
            });
        }
    }
}