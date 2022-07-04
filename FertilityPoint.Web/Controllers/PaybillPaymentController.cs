using FertilityPoint.BLL.Repositories.MpesaStkModule;
using FertilityPoint.DTO.MpesaC2BModule;
using FertilityPoint.DTO.MpesaStkModule;
using FertilityPoint.Web.Services.MpesaC2BModule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FertilityPoint.Controllers
{
    public class PaybillPaymentController : Controller
    {
        private readonly MpesaApiConfiguration mpesaApiConfiguration;

        private readonly IMpesaClient mpesaClient;

        private readonly IPaymentRepository paymentRepository;

        private readonly ILogger<PaybillPaymentController> logger;
        //private readonly LinkGenerator _linkGenerator;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IMemoryCache _memoryCache;
        public PaybillPaymentController(IPaymentRepository paymentRepository,IOptions<MpesaApiConfiguration> mpesaApiConfiguration, IMpesaClient mpesaClient, ILogger<PaybillPaymentController> logger)
        {
            this.mpesaClient = mpesaClient;

            this.logger = logger;

            this.paymentRepository = paymentRepository;

            this.mpesaApiConfiguration = mpesaApiConfiguration.Value;
        }
        public IActionResult C2BPayment()
        {
            return View();
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> C2BPayment(CustomerToBusinessViewModel customerToBusinessViewModel)
        {
            MpesaResponse c2BResults;

            try
            {
                var key = "AxNwrRq9ICkc10ACgcpQ6Yak3v8tffTO";

                var secrete = "8Ig25DvrK9IQ8pPR";

                var c2BPayment = new CustomerToBusinessSimulate
                (
                    shortCode: mpesaApiConfiguration.C2BShortCode,

                    commandId: Transaction_Type.CustomerPayBillOnline,

                    amount: customerToBusinessViewModel.Amount,

                    msisdn: mpesaApiConfiguration.C2BMSISDNTest,

                    billReferenceNumber: customerToBusinessViewModel.PaymentReference
                );

                c2BResults = await mpesaClient.MakeC2BPayment(c2BPayment, await generateAccessToken(key,secrete), MpesaRequestEndpoint.CustomerToBusinessSimulate);
            }
            catch (MpesaAPIException ex)
            {
                logger.LogError(ex, ex.Message);
                return View().WithDanger("Error", ex.Message);
            }
            CustomerToBusinessCallback customerToBusinessCallback = new CustomerToBusinessCallback();

            paymentRepository.SaveLipaNaMpesa(customerToBusinessCallback);

            return View().WithSuccess("Success", "You have successfully submitted C2B payment.");
        }

        public IActionResult C2BRegister()
        {
            return View();
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> C2BRegister(CustomerToBusinessRegisterViewModel customerToBusinessRegisterViewModel)
        {
            MpesaResponse c2BRegisterResults;

            try
            {
                var key = "AxNwrRq9ICkc10ACgcpQ6Yak3v8tffTO";

                var secrete = "8Ig25DvrK9IQ8pPR";

                var c2BRegisterCallback = new CustomerToBusinessRegisterUrl
                    (
                        shortCode: mpesaApiConfiguration.C2BShortCode,

                        responseType: customerToBusinessRegisterViewModel.customerToBusinessResponse.ToString(),

                        confirmationUrl: customerToBusinessRegisterViewModel.ConfirmationUrl,

                        validationUrl: customerToBusinessRegisterViewModel.ValidationUrl
                    );

                c2BRegisterResults = await mpesaClient.RegisterC2BUrl(c2BRegisterCallback, await generateAccessToken(key, secrete), MpesaRequestEndpoint.RegisterC2BUrl);
            }
            catch (MpesaAPIException ex)
            {
                logger.LogError(ex, ex.Message);

                return View().WithDanger("Error", ex.Message);
            }

            return View().WithSuccess("Success", "Successfully added C2B confirmation and validation URLs");
        }

        public async Task<string> generateAccessToken(string key, string secrete)
        {
            try
            {
                var url = @"https://sandbox.safaricom.co.ke/oauth/v1/generate?grant_type=client_credentials";

                HttpClient client = new HttpClient();

                var byteArray = System.Text.Encoding.ASCII.GetBytes(key + ":" + secrete);

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                HttpResponseMessage response = await client.GetAsync(url);

                HttpContent content = response.Content;

                string result = await content.ReadAsStringAsync();

                var mpesaAccessToken = JsonConvert.DeserializeObject<MpesaAccessTokenDTO>(result);

                return mpesaAccessToken.access_token;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

    }
}
