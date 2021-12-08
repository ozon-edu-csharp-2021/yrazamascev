using Confluent.Kafka;

using Microsoft.Extensions.Options;

using OzonEdu.MerchApi.Domain.Infrastructure.Configuration;

using System;

namespace OzonEdu.MerchApi.Domain.Infrastructure.MessageBroker
{
    public class ProducerBuilderWrapper : IProducerBuilderWrapper
    {
        public IProducer<string, string> Producer { get; set; }
        public string StockReshippedTopic { get; set; }

        public ProducerBuilderWrapper(IOptions<KafkaConfiguration> configuration)
        {
            KafkaConfiguration configValue = configuration.Value;
            if (configValue is null)
            {
                throw new ApplicationException("Configuration for kafka server was not specified");
            }

            ProducerConfig producerConfig = new()
            {
                BootstrapServers = configValue.BootstrapServers
            };

            Producer = new ProducerBuilder<string, string>(producerConfig).Build();
            //StockReshippedTopic = configValue.Topic;
        }
    }
}