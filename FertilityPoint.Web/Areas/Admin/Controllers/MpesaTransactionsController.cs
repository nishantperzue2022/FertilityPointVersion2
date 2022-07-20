using FertilityPoint.BLL.Repositories.MpesaStkModule;
using FertilityPoint.BLL.Repositories.ServiceModule;
using FertilityPoint.DTO.MpesaStkModule;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FertilityPoint.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MpesaTransactionsController : Controller
    {
        private readonly IPaymentRepository paymentRepository;

        private readonly IServicesRepository servicesRepository;


        public MpesaTransactionsController(IServicesRepository servicesRepository, IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;

            this.servicesRepository = servicesRepository;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var payments = await paymentRepository.GetAll();

                return View(payments);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }

        public IActionResult ValidatePayment()
        {
            try
            {

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }

        public async Task<IActionResult> GetByTransactionId(string TransactionId)
        {
            try
            {
                var payment = await paymentRepository.GetByTransNumber(TransactionId.Trim());

                if (payment != null)
                {
                    MpesaPaymentDTO mpesaPaymentDTO = new MpesaPaymentDTO
                    {
                        Id = payment.Id,

                        ReceiptNo = payment.ReceiptNo,

                        Amount = payment.Amount,                       

                        TransactionNumber = payment.TransactionNumber,

                        PhoneNumber = payment.PhoneNumber,

                        TransactionDate = payment.TransactionDate,
                    };

                    return Json(new { data = payment });
                }
                return Json(new { data = false });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }

        public async Task<IActionResult> CreatePayment()
        {
            try
            {
                var services = await servicesRepository.GetAll();

                ViewBag.Services = services;

                return View();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            };
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment(MpesaPaymentDTO mpesaPaymentDTO)
        {
            try
            {
                var getTransaction = (await paymentRepository.GetByTransNumber(mpesaPaymentDTO.TransactionNumber.Trim()));

                if (getTransaction != null)
                {
                    return Json(new { success = false, responseText = "Sorry ,This transaction has already been captured" });

                }

                var validateServiceCharge = await servicesRepository.GetById(mpesaPaymentDTO.ServiceId);

                if (mpesaPaymentDTO.Amount < validateServiceCharge.Amount)
                {
                    return Json(new { success = false, responseText = "Sorry ! You have entered less amount for this service" });

                }

                var result = await paymentRepository.CreateMpesaPayment(mpesaPaymentDTO);

                if (result != null)
                {
                    return Json(new { success = true, responseText = "Transaction has been saved successfully" });
                }
                else
                {
                    return Json(new { success = false, responseText = "Failed to save transaction" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
    }
}
