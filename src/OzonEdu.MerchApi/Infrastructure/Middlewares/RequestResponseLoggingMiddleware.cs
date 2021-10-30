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
        private const int HEADER_PADDING = -30;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            StringBuilder logBuilder = new();
            await LogRequest(context, logBuilder);
            logBuilder.Clear();
            await LogResponse(context, logBuilder);
        }

        private void AddHttpHeaders(StringBuilder logBuilder, string header, IHeaderDictionary httpHeaders)
        {
            if (httpHeaders.Count > 0)
            {
                logBuilder.AppendLine(header);
                foreach (KeyValuePair<string, StringValues> httpHeader in httpHeaders)
                {
                    AddHttpInformation(logBuilder, httpHeader.Key, httpHeader.Value);
                }
            }
        }

        private void AddHttpInformation(StringBuilder logBuilder, string header, object value)
        {
            logBuilder.AppendLine($"\t{header,HEADER_PADDING}{value}");
        }

        private async Task LogRequest(HttpContext context, StringBuilder logBuilder)
        {
            AddHttpHeaders(logBuilder, "Request headers:", context.Request.Headers);

            logBuilder.AppendLine("Request information:");
            AddHttpInformation(logBuilder, "Schema", context.Request.Scheme);
            AddHttpInformation(logBuilder, "Path", context.Request.Path);
            AddHttpInformation(logBuilder, "QueryString", context.Request.QueryString);

            _logger.LogInformation(logBuilder.ToString());
            logBuilder.Clear();

            try
            {
                if (context.Request.ContentLength > 0)
                {
                    context.Request.EnableBuffering();

                    byte[] buffer = new byte[context.Request.ContentLength.Value];
                    await context.Request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length));
                    string bodyAsText = Encoding.UTF8.GetString(buffer);

                    logBuilder.AppendLine("Request body:");
                    logBuilder.AppendLine(bodyAsText);
                    _logger.LogInformation(logBuilder.ToString());

                    context.Request.Body.Position = 0;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not log request body");
            }
        }

        private async Task LogResponse(HttpContext context, StringBuilder logBuilder)
        {
            Stream originalBody = context.Response.Body;
            using MemoryStream newBody = new();
            context.Response.Body = newBody;

            await _next(context);

            AddHttpHeaders(logBuilder, "Response headers:", context.Response.Headers);

            _logger.LogInformation(logBuilder.ToString());
            logBuilder.Clear();

            try
            {
                newBody.Seek(0, SeekOrigin.Begin);
                string bodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();

                logBuilder.AppendLine("Response body:");
                logBuilder.AppendLine(bodyText);
                _logger.LogInformation(logBuilder.ToString());

                newBody.Seek(0, SeekOrigin.Begin);
                await newBody.CopyToAsync(originalBody);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not log response body");
            }
        }
    }
}