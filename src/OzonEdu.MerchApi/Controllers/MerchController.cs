using MediatR;

using Microsoft.AspNetCore.Mvc;

using OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate;
using OzonEdu.MerchApi.Domain.Infrastructure.Commands.CreateMerchOrder;
using OzonEdu.MerchApi.Domain.Infrastructure.Commands.GetMerchOrders;
using OzonEdu.MerchApi.HttpModels;
using OzonEdu.MerchApi.Services.Interfaces;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Controllers
{
    [Route("api/merch")]
    [Produces("application/json")]
    public class MerchController : ControllerBase
    {
        private readonly IMerchService _service;
        private readonly IMediator _mediator;

        public MerchController(IMerchService service, IMediator mediator)
        {
            _service = service;
            _mediator = mediator;
        }

        [HttpPost("get-merch-orders")]
        public async Task<ActionResult<GetMerchOrdersResponse>> GetMerchOrders(GetMerchOrdersRequest request, CancellationToken token)
        {
            GetMerchOrdersCommand command = new() { EmployeeId = request.EmployeeId };

            List<MerchOrder> merchOrders = await _mediator.Send(command, token);

            GetMerchOrdersResponse response = new()
            {
                MerchOrders = new List<MerchOrderViewModel>()
            };

            foreach (MerchOrder merchOrder in merchOrders)
            {
                response.MerchOrders.Add(new MerchOrderViewModel()
                {
                    DoneAt = merchOrder.DoneAt.Value,
                    RequestType = merchOrder.RequestType.Name,
                    EmployeeId = merchOrder.EmployeeId,
                    ReserveAt = merchOrder.ReserveAt.Value,
                    Status = merchOrder.Status.Name,
                    Type = merchOrder.Type.Name,
                });
            }

            return Ok(response);
        }

        [HttpPost("issue-merch")]
        public async Task<ActionResult<int>> IssueMerch(IssueMerchRequest request, CancellationToken token)
        {
            CreateManualMerchOrderCommand command = new() { EmployeeId = request.EmployeeId };

            int merchId = await _mediator.Send(command, token);

            return Ok(merchId);
        }
    }
}