﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using OzonEdu.MerchandiseService.Infrastructure.Middlewares;

using System;

namespace OzonEdu.MerchandiseService.Infrastructure.StartupFilters
{
    public class TerminalStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.Map("/version", builder => builder.UseMiddleware<VersionMiddleware>());
                app.Map("/ready", builder => builder.UseMiddleware<ReadyMiddleware>());
                app.Map("/live", builder => builder.UseMiddleware<ReadyMiddleware>());
                app.UseMiddleware<RequestLoggingMiddleware>();
                next(app);
            };
        }
    }
}