using Confluent.Kafka;

namespace OzonEdu.MerchApi.Domain.Infrastructure.MessageBroker
{
    public interface IProducerBuilderWrapper
    {
        IProducer<string, string> Producer { get; set; }
        string StockReshippedTopic { get; set; }
        string EmployeeIssueMerchTopic { get; set; }
    }
}