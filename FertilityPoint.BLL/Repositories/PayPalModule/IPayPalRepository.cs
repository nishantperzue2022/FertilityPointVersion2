using FertilityPoint.DTO.PayPalModule;

namespace FertilityPoint.BLL.Repositories.PayPalModule
{
    public interface IPayPalRepository
    {
        Task<PaypalPaymentDTO> Create(PaypalPaymentDTO payPalDTO);
        Task<List<PaypalPaymentDTO>> GetAll();
    }
}