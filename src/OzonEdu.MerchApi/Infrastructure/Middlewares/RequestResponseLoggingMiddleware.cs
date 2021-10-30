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
        private readonly StringBuilder _logBuilder = new();
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
            _logBuilder.Clear();
            await LogResponse(context);
            _logBuilder.Clear();
        }

        private void AddHttpHeaders(string header, IHeaderDictionary httpHeaders)
        {
            if (httpHeaders.Count > 0)
            {
                _logBuilder.AppendLine(header);
                foreach (KeyValuePair<string, StringValues> httpHeader in httpHeaders)
                {
                    _logBuilder.AppendLine($"\t{httpHeader.Key,HEADER_PADDING}{httpHeader.Value}");
                }
            }
        }

        private void AddHttpInformation(string header, object value)
        {
            _logBuilder.AppendLine($"\t{header,HEADER_PADDING}{value}");
        }

        private async Task LogRequest(HttpContext context)
        {
            AddHttpHeaders("Request headers:", context.Request.Headers);

            _logBuilder.AppendLine("Request information:");
            AddHttpInformation("Schema", context.Request.Scheme);
            AddHttpInformation("Path", context.Request.Path);
            AddHttpInformation("QueryString", context.Request.QueryString);

            _logger.LogInformation(_logBuilder.ToString());
            _logBuilder.Clear();

            try
            {
                if (context.Request.ContentLength > 0)
                {
                    context.Request.EnableBuffering();

                    byte[] buffer = new byte[context.Request.ContentLength.Value];
                    await context.Request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length));
                    string bodyAsText = Encoding.UTF8.GetString(buffer);

                    _logBuilder.AppendLine("Request body:");
                    _logBuilder.AppendLine(bodyAsText);
                    _logger.LogInformation(_logBuilder.ToString());

                    context.Request.Body.Position = 0;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not log request body");
            }
        }

        private async Task LogResponse(HttpContext context)
        {
            Stream originalBody = context.Response.Body;
            using MemoryStream newBody = new();
            context.Response.Body = newBody;

            await _next(context);

            AddHttpHeaders("Response headers:", context.Response.Headers);

            _logger.LogInformation(_logBuilder.ToString());
            _logBuilder.Clear();

            try
            {
                newBody.Seek(0, SeekOrigin.Begin);
                string bodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();

                _logBuilder.AppendLine("Response body:");
                _logBuilder.AppendLine(bodyText);
                _logger.LogInformation(_logBuilder.ToString());

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