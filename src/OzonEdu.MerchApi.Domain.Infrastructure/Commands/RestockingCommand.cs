using MediatR;

using System.Collections.Generic;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Commands
{
    public class RestockingCommand : IRequest
    {
        public IReadOnlyCollection<long> Skus { get; set; }
    }
}