using FertilityPoint.DTO.MpesaC2BModule;
using System.Threading;
using System.Threading.Tasks;

namespace FertilityPoint.Web.Services.MpesaC2BModule
{
    public interface IMpesaClient
    {
        Task<MpesaResponse> RegisterC2BUrl(CustomerToBusinessRegisterUrl customerToBusinessRegisterUrl, string accesstoken, string mpesaRequestEndpoint, CancellationToken cancellationToken = default);
        Task<MpesaResponse> MakeC2BPayment(CustomerToBusinessSimulate customerToBusinessSimulate, string accesstoken, string mpesaRequestEndpoint, CancellationToken cancellationToken = default);

    }
}
