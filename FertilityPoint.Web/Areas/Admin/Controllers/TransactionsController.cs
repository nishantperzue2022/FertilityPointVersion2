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
    public class TransactionsController : Controller
    {
        private readonly IPaymentRepository paymentRepository;

        private readonly IServicesRepository servicesRepository;


        public TransactionsController(IServicesRepository servicesRepository, IPaymentRepository paymentRepository)
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

                if(getTransaction != null)
                {
                    return Json(new { success = false, responseText = "Sorry ,This transaction has already been captured" });

                }

                var validateServiceCharge = await servicesRepository.GetById(mpesaPaymentDTO.ServiceId);

                if (mpesaPaymentDTO.Amount < validateServiceCharge.Amount )
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
