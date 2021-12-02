using Confluent.Kafka;

using CSharpCourse.Core.Lib.Events;

using MediatR;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using OzonEdu.MerchApi.Domain.Infrastructure.Configuration;

using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.StockApi.HostedServices
{
    public class SupplyConsumerHostedService : BackgroundService
    {
        private readonly KafkaConfiguration _config;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<SupplyConsumerHostedService> _logger;

        public SupplyConsumerHostedService(
            IOptions<KafkaConfiguration> config,
            IServiceScopeFactory scopeFactory,
            ILogger<SupplyConsumerHostedService> logger)
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

            using IConsumer<Ignore, string> c = new ConsumerBuilder<Ignore, string>(config).Build();
            //c.Subscribe(_config.StockTopic);
            c.Subscribe(_config.EmployeeTopic);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using IServiceScope scope = _scopeFactory.CreateScope();

                    try
                    {
                        await Task.Yield();
                        IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                        ConsumeResult<Ignore, string> cr = c.Consume(stoppingToken);

                        if (cr is not null)
                        {
                            SupplyShippedEvent message = JsonSerializer.Deserialize<SupplyShippedEvent>(cr.Message.Value);

                            //await mediator.Send(new ReplenishStockCommand()
                            //{
                            //    SupplyId = message.SupplyId,
                            //    Items = message.Items.Select(it => new StockItemQuantityDto()
                            //    {
                            //        Quantity = (int)it.Quantity,
                            //        Sku = it.SkuId
                            //    }).ToArray()
                            //}, stoppingToken);
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
                c.Commit();
                c.Close();
            }
        }
    }
}