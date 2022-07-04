using FertilityPoint.DTO.MpesaC2BModule;
using FertilityPoint.DTO.MpesaStkModule;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FertilityPoint.BLL.Repositories.MpesaStkModule
{
    public interface IPaymentRepository
    {
        Task<CheckoutRequestDTO> SaveCheckoutRequest(CheckoutRequestDTO checkoutRequestDTO);
        Task<List<MpesaPaymentDTO>> GetAll();
        Task<MpesaPaymentDTO> GetByTransId(string TransactionId);
        CheckoutRequestDTO GetCheckOutRequestById(string CheckoutRequestID);
        void SaveSTKCallBackResponse(MpesaPaymentDTO mpesaPaymentDTO);
        string GetTotalRevenue();
        bool IsTransactionExists(string TransactionNumber);
        void SaveLipaNaMpesa(CustomerToBusinessCallback  customerToBusinessCallback);
    }
}