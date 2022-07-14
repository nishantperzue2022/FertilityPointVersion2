

using AutoMapper;
using FertilityPoint.DAL.Modules;
using FertilityPoint.DTO.PayPalModule;
using Microsoft.EntityFrameworkCore;

namespace FertilityPoint.BLL.Repositories.PayPalModule
{
    public class PayPalRepository : IPayPalRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;

        public PayPalRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;

            this.mapper = mapper;
        }

        public async Task<PaypalPaymentDTO> Create(PaypalPaymentDTO payPalDTO)
        {
            try
            {  
                var paypalPayment = mapper.Map<PaypalPayment>(payPalDTO);

                context.PaypalPayments.Add(paypalPayment);

                await context.SaveChangesAsync();

                return payPalDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<List<PaypalPaymentDTO>> GetAll()
        {
            try
            {
                var data = await context.PaypalPayments.ToListAsync();

                var paypalPayment = mapper.Map<List<PaypalPayment>, List<PaypalPaymentDTO>>(data);

                return paypalPayment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
    }
}
