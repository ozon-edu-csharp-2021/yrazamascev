using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Infrastructure.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await LogRequest(context);
            await LogResponse(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            foreach (KeyValuePair<string, StringValues> header in context.Response.Headers)
            {
                _logger.LogInformation("Request headers logged");
                _logger.LogInformation($"{header.Key}:{header.Value}");
            }

            _logger.LogInformation($"Http Request Information:{Environment.NewLine}" +
                                   $"Schema:{context.Request.Scheme} " +
                                   $"Host: {context.Request.Host} " +
                                   $"Path: {context.Request.Path} " +
                                   $"QueryString: {context.Request.QueryString} ");

            try
            {
                if (context.Request.ContentLength > 0)
                {
                    context.Request.EnableBuffering();

                    byte[] buffer = new byte[context.Request.ContentLength.Value];
                    await context.Request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length));
                    string bodyAsText = Encoding.UTF8.GetString(buffer);
                    _logger.LogInformation("Request logged");
                    _logger.LogInformation($"Request Body:bodyAsText");

                    context.Request.Body.Position = 0;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not log request");
            }
        }

        private async Task LogResponse(HttpContext context)
        {
            Stream originalBody = context.Response.Body;
            using MemoryStream newBody = new();
            context.Response.Body = newBody;

            await _next(context);

            foreach (KeyValuePair<string, StringValues> header in context.Response.Headers)
            {
                _logger.LogInformation("Response headers logged");
                _logger.LogInformation($"{header.Key}:{header.Value}");
            }

            try
            {
                newBody.Seek(0, SeekOrigin.Begin);
                string bodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                _logger.LogInformation($"Response: {bodyText}");
                newBody.Seek(0, SeekOrigin.Begin);
                await newBody.CopyToAsync(originalBody);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not log request body");
            }
        }
    }
}