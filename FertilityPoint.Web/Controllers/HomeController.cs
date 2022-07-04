using FertilityPoint.BLL.Repositories.MpesaStkModule;
using FertilityPoint.DTO.MpesaStkModule;
using FertilityPoint.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SkiCareHMS.Data.DTOs.MpesaStkModule;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FertilityPoint.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPaymentRepository paymentRepository;

        private readonly ILogger<HomeController> _logger;
        public HomeController(IPaymentRepository paymentRepository,ILogger<HomeController> logger)
        {
            _logger = logger;

            this.paymentRepository = paymentRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public void SaveCallBack([FromBody] DarajaResponseAfterUserEntersPin darajaResponse)
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

                    //var getCheckoutdetails = paymentService.GetCheckOutRequestResponseById(transaction.CheckoutRequestID);

                    //var getPatient = patientService.GetById(getCheckoutdetails.PatientId);

                    //transaction.FirstName = getPatient.FirstName;

                    //transaction.LastName = getPatient.LastName;

                    paymentRepository.SaveSTKCallBackResponse(transaction);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                // return null;
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
