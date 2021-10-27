﻿using Grpc.Core;
using Grpc.Core.Interceptors;

using Microsoft.Extensions.Logging;

using System.Text.Json;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Infrastructure.Interceptors
{
    public class LoggingInterceptor : Interceptor
    {
        private readonly ILogger<LoggingInterceptor> _logger;

        public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
        {
            _logger = logger;
        }

        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            string requestJson = JsonSerializer.Serialize(request);
            _logger.LogInformation(requestJson);

            Task<TResponse> response = base.UnaryServerHandler(request, context, continuation);

            string responseJson = JsonSerializer.Serialize(response);
            _logger.LogInformation(responseJson);

            return response;
        }
    }
}