using Confluent.Kafka;

using CSharpCourse.Core.Lib.Enums;
using CSharpCourse.Core.Lib.Events;

using MediatR;

using OzonEdu.MerchApi.Domain.Events;
using OzonEdu.MerchApi.Domain.Infrastructure.MessageBroker;

using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Handlers
{
    public class EmployeeIssueMerchEventHandler : INotificationHandler<EmployeeIssueMerchEvent>
    {
        private readonly IProducerBuilderWrapper _producerBuilderWrapper;

        public EmployeeIssueMerchEventHandler(IProducerBuilderWrapper producerBuilderWrapper)
            => _producerBuilderWrapper = producerBuilderWrapper;

        public Task Handle(EmployeeIssueMerchEvent notification, CancellationToken cancellationToken)
        {
            _producerBuilderWrapper.Producer.Produce(_producerBuilderWrapper.EmployeeIssueMerchTopic,
                new Message<string, string>()
                {
                    Key = notification.EmployeeEmail,
                    Value = JsonSerializer.Serialize(new NotificationEvent()
                    {
                        EmployeeEmail = notification.EmployeeEmail,
                        EventType = EmployeeEventType.MerchDelivery
                    })
                });

            return Task.CompletedTask;
        }
    }
}