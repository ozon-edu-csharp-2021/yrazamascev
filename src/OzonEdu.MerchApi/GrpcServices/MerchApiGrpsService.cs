using Grpc.Core;

using OzonEdu.MerchApi.Grpc;
using OzonEdu.MerchApi.Services.Interfaces;

using System.Threading.Tasks;

namespace OzonEdu.MerchApi.GrpcServices
{
    public class MerchApiGrpsService : MerchApiGrpc.MerchApiGrpcBase
    {
        private readonly IMerchService _service;

        public MerchApiGrpsService(IMerchService service) => _service = service;

        public override async Task<CheckWasIssuedMerchResponse> CheckWasIssuedMerch(CheckWasIssuedMerchRequest request, ServerCallContext context)
        {
            bool response = await _service.GetMerchOrders(request.EmployeeId, context.CancellationToken);

            return new CheckWasIssuedMerchResponse()
            {
                EmployeeId = request.EmployeeId,
                WasIssued = response
            };
        }

        public override async Task<IssueMerchResponse> IssueMerch(IssueMerchRequest request, ServerCallContext context)
        {
            bool response = await _service.IssueMerch(request.EmployeeId, context.CancellationToken);

            return new IssueMerchResponse()
            {
                EmployeeId = request.EmployeeId
            };
        }
    }
}