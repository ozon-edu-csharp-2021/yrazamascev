﻿using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.Infrastructure.Swagger
{
    public class HeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();
            operation.Parameters.Add(new OpenApiParameter
            {
                In = ParameterLocation.Header,
                Name = "our-header",
                Required = false,
                Schema = new OpenApiSchema { Type = "string" }
            });
        }
    }
}