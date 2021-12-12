using MediatR;

using Microsoft.AspNetCore.Mvc;

using OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate;
using OzonEdu.MerchApi.Domain.Infrastructure.Commands;
using OzonEdu.MerchApi.HttpModels;
using OzonEdu.MerchApi.HttpModels.Helpers;
using OzonEdu.MerchApi.Infrastructure.Extensions;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Controllers
{
    [Route("api/merch")]
    [Produces("application/json")]
    public class MerchController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MerchController(IMediator mediator) => _mediator = mediator;

        [HttpPost("get-merch-orders")]
        public async Task<ActionResult<GetMerchOrdersResponse>> GetMerchOrders(GetMerchOrdersRequest request, CancellationToken token)
        {
            GetMerchOrdersCommand command = new()
            {
                EmployeeEmail = request.EmployeeEmail
            };

            IReadOnlyCollection<MerchOrder> merchOrders = await _mediator.Send(command, token);
            GetMerchOrdersResponse response = new()
            {
                MerchOrders = merchOrders.Map(entity => HttpModelsMapper.MerchOrderToViewModel(entity)).ToList()
            };

            return Ok(response);
        }

        [HttpPost("issue-merch")]
        public async Task<ActionResult<IssueMerchResponse>> IssueMerch(IssueMerchRequest request, CancellationToken token)
        {
            CreateMerchOrderCommand command = new()
            {
                EmployeeEmail = request.EmployeeEmail,
                ClothingSize = request.ClothingSize,
                MerchType = request.MerchType,
                MerchRequestType = MerchRequestType.Manual
            };

            MerchOrder merchOrder = await _mediator.Send(command, token);
            IssueMerchResponse response = new()
            {
                MerchOrder = HttpModelsMapper.MerchOrderToViewModel(merchOrder)
            };

            return Ok(response);
        }
    }
}