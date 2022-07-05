using FertilityPoint.BLL.Repositories.MpesaStkModule;
using FertilityPoint.DAL.Modules;
using FertilityPoint.DTO.MpesaC2BModule;
using FertilityPoint.DTO.MpesaStkModule;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SkiCareHMS.Data.DTOs.MpesaStkModule;
using System;
using System.Linq;
using System.Text.Json;

namespace FertilityPoint.Controllers
{
    public class OnlinePaymentController : Controller
    {
        private readonly IPaymentRepository paymentRepository;

        private readonly ApplicationDbContext context;

        private readonly IWebHostEnvironment env;
        public OnlinePaymentController(IWebHostEnvironment env, ApplicationDbContext context, IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;

            this.context = context;

            this.env = env;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public void ValidationUrl([FromBody] CustomerToBusinessCallbackDTO response)
        {


        }

        [HttpPost]
        public async void FertitlityPointPayBillConfirmation([FromBody] CustomerToBusinessCallbackDTO response)
        {
            try
            {
                var mpesaResponse = new MpesaPaymentDTO
                {
                    BillRefNumber = response.BillRefNumber,

                    BusinessShortCode = response.BusinessShortCode,

                    FirstName = response.FirstName,

                    MiddleName = response.MiddleName,

                    LastName = response.LastName,

                    InvoiceNumber = response.InvoiceNumber,

                    PhoneNumber = response.MSISDN,

                    OrgAccountBalance = response.OrgAccountBalance,

                    ThirdPartyTransID = response.ThirdPartyTransID,

                    Amount = Convert.ToDecimal(response.TransAmount),

                    TransactionNumber = response.TransID,

                    TransactionType = response.TransactionType,

                    TransactionDate = response.TransTime,

                };

                var result = paymentRepository.SavePayBillCallBackResponse(mpesaResponse);

              
                    var filename = $"{mpesaResponse.PhoneNumber ??= "Error"}.json";

                    var rootPath = Path.Combine(env.WebRootPath, "Application_Files\\C2BConfirmationResults\\");
                    // To check if directory exists. If the directory does not exists we create a new directory
                    if (!Directory.Exists(rootPath))
                    {
                        Directory.CreateDirectory(rootPath);
                    }
                    // Get the path of filename
                    var filePath = Path.Combine(env.WebRootPath, "Application_Files\\C2BConfirmationResults\\", filename);

                    await System.IO.File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(mpesaResponse, Formatting.Indented));
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public void FertitlityPointSTKCallBack([FromBody] DarajaResponseAfterUserEntersPin darajaResponse)
        {
            try
            {
                if (darajaResponse.Body.stkCallback.ResultCode == 0)
                {
                    var transaction = new MpesaPaymentDTO
                    {
                        CheckoutRequestID = darajaResponse.Body.stkCallback.CheckoutRequestID,

                        MerchantRequestID = darajaResponse.Body.stkCallback.MerchantRequestID,

                        ResultCode = darajaResponse.Body.stkCallback.ResultCode,

                        ResultDesc = darajaResponse.Body.stkCallback.ResultDesc,

                        Amount = Convert.ToDecimal(darajaResponse.Body.stkCallback.CallbackMetadata.Item.Where(p => p.Name.Contains("Amount")).FirstOrDefault().Value.ToString()),

                        TransactionNumber = darajaResponse.Body.stkCallback.CallbackMetadata.Item.Where(p => p.Name.Contains("MpesaReceiptNumber")).FirstOrDefault().Value.ToString(),

                        TransactionDate = darajaResponse.Body.stkCallback.CallbackMetadata.Item.Where(p => p.Name.Contains("TransactionDate")).FirstOrDefault().Value.ToString(),

                        PhoneNumber = darajaResponse.Body.stkCallback.CallbackMetadata.Item.Where(p => p.Name.Contains("PhoneNumber")).FirstOrDefault().Value.ToString(),

                    };

                    paymentRepository.SaveSTKCallBackResponse(transaction);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                // return null;
            }
        }


    }
}
