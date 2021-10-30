using Microsoft.AspNetCore.Mvc;

using OzonEdu.MerchApi.DTO;
using OzonEdu.MerchApi.Services.Interfaces;

using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Controllers
{
    [Route("api/merch")]
    [Produces("application/json")]
    public class MerchController : ControllerBase
    {
        private readonly IMerchService _service;

        public MerchController(IMerchService service)
        {
            _service = service;
        }

        [HttpPost("check-was-issued-merch")]
        public async Task<ActionResult<CheckWasIssuedMerchResponse>> CheckWasIssuedMerch(CheckWasIssuedMerchRequest request, CancellationToken token)
        {
            bool response = await _service.CheckWasIssuedMerch(request.EmployeeId, token);

            return Ok(new CheckWasIssuedMerchResponse
            {
                EmployeeId = request.EmployeeId,
                WasIssued = response
            });
        }

        [HttpPost("issue-merch")]
        public async Task<ActionResult<IssueMerchResponse>> IssueMerch(IssueMerchRequest request, CancellationToken token)
        {
            bool response = await _service.IssueMerch(request.EmployeeId, token);

            return Ok(new IssueMerchResponse()
            {
                EmployeeId = request.EmployeeId
            });
        }
    }
}