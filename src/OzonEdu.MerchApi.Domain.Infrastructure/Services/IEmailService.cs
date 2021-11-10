using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Services
{
    public interface IEmailService
    {
        Task<bool> SendMail(long employeeId, CancellationToken cancellationToken);
    }
}