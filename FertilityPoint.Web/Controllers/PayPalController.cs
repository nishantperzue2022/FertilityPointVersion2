using FertilityPoint.BLL.Repositories.PayPalModule;
using FertilityPoint.DTO.PayPalModule;
using FertilityPoint.PayPal.PaypalHelper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FertilityPoint.Web.Controllers
{
    //[Route("PayPal")]
    public class PayPalController : Controller
    {
        public IConfiguration configuration { get; }

        private readonly IPayPalRepository payPalRepository;
        public PayPalController(IConfiguration configuration, IPayPalRepository payPalRepository)
        {
            this.configuration = configuration;

            this.payPalRepository = payPalRepository;
        }

        //[Route("index")]
        //[Route("")]
        //[Route("~/")]
        public IActionResult Index()
        {
            var productDetailsDTO = new ProductDetailsDTO();

            var data = productDetailsDTO.FindAll();

            ViewBag.TotalAmount = Convert.ToDecimal(data.Sum(x => x.Amount * x.Quantity));

            return View(data);
        }

        public IActionResult Success()
        {

            return View();
        }

        public IActionResult Cancel()
        {
            return View();
        }

        [HttpPost]
        [Route("checkout")]
        public IActionResult Checkout(double total)
        {
            try
            {
                var payPalAPI = new PayPalAPI(configuration);

                string url = payPalAPI.GetRedirectUrlToPayPal(total, "USD");

                return Redirect(url);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);

                return null;
            }
        }

        [Route("success")]
        public async Task<IActionResult> Success([FromQuery(Name = "PaymentId")] string paymentId, [FromQuery(Name = "PayerId")] string payerId)
        {
            try
            {
                var payPalAPI = new PayPalAPI(configuration);

                PayPalPaymentExecutedResponse result = await payPalAPI.ExecutedPayment(paymentId, payerId);

                PaypalPaymentDTO paypalPaymentDTO = new PaypalPaymentDTO();

                {
                    paypalPaymentDTO.TransactionId = result.id;

                    paypalPaymentDTO.Cart = result.cart;

                    paypalPaymentDTO.Intent = result.intent;

                    paypalPaymentDTO.State = result.state;

                    paypalPaymentDTO.TransactionDate = result.create_time;

                    paypalPaymentDTO.PayerCountryCode = result.payer.payer_info.country_code;

                    paypalPaymentDTO.PayerFirstName = result.payer.payer_info.first_name;

                    paypalPaymentDTO.PayerLastName = result.payer.payer_info.last_name;

                    paypalPaymentDTO.PayerEmail = result.payer.payer_info.email;

                    paypalPaymentDTO.PayerId = result.payer.payer_info.payer_id;

                    paypalPaymentDTO.PaymentMethod = result.payer.payment_method;

                    paypalPaymentDTO.Status = result.payer.status;

                    foreach (var item in result.transactions)
                    {
                        paypalPaymentDTO.Currency = item.amount.currency;

                        paypalPaymentDTO.AmountPaid = Convert.ToDouble(item.amount.total);

                        paypalPaymentDTO.RecipientEmail = item.payee.email;

                        paypalPaymentDTO.MerchantId = item.payee.merchant_id;

                        paypalPaymentDTO.RecipientCity = item.item_list.shipping_address.city;

                        paypalPaymentDTO.RecipientCountryCode = item.item_list.shipping_address.country_code;

                        paypalPaymentDTO.Line1 = item.item_list.shipping_address.line1;

                        paypalPaymentDTO.RecipientPostalCode = item.item_list.shipping_address.postal_code;

                        paypalPaymentDTO.RecipientName = item.item_list.shipping_address.recipient_name;

                    }
                }

                var postTransaction = await payPalRepository.Create(paypalPaymentDTO);

                ViewBag.result = paypalPaymentDTO;

                return View("Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

    }
}
