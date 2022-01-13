using Confluent.Kafka;

using CSharpCourse.Core.Lib.Enums;
using CSharpCourse.Core.Lib.Events;

using MediatR;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate;
using OzonEdu.MerchApi.Domain.Infrastructure.Commands;
using OzonEdu.MerchApi.Domain.Infrastructure.Configuration;

using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.StockApi.HostedServices
{
    public class EmployeeConsumerHostedService : BackgroundService
    {
        private readonly KafkaConfiguration _config;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<EmployeeConsumerHostedService> _logger;

        public EmployeeConsumerHostedService(
            IOptions<KafkaConfiguration> config,
            IServiceScopeFactory scopeFactory,
            ILogger<EmployeeConsumerHostedService> logger)
        {
            _config = config.Value;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ConsumerConfig config = new()
            {
                GroupId = _config.GroupId,
                BootstrapServers = _config.BootstrapServers,
            };

            using IConsumer<Ignore, string> consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_config.EmployeeTopic);

            try
            {
                while (stoppingToken.IsCancellationRequested == false)
                {
                    using IServiceScope scope = _scopeFactory.CreateScope();

                    try
                    {
                        await Task.Yield();
                        IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                        ConsumeResult<Ignore, string> result = consumer.Consume(stoppingToken);

                        if (result is not null)
                        {
                            NotificationEvent message = JsonSerializer.Deserialize<NotificationEvent>(result.Message.Value);

                            if (message.EventType is EmployeeEventType.Hiring
                                or EmployeeEventType.ProbationPeriodEnding
                                or EmployeeEventType.ConferenceAttendance
                                && message.Payload is MerchDeliveryEventPayload payload)
                            {
                                await mediator.Send(new CreateMerchOrderCommand()
                                {
                                    EmployeeEmail = message.EmployeeEmail,
                                    ClothingSize = payload.ClothingSize,
                                    MerchType = payload.MerchType,
                                    MerchRequestType = MerchRequestType.Auto
                                }, stoppingToken);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error while get consume. Message {ex.Message}");
                    }
                }
            }
            finally
            {
                consumer.Commit();
                consumer.Close();
            }
        }
    }
}