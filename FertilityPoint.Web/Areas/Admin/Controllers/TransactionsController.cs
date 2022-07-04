using FertilityPoint.BLL.Repositories.MpesaStkModule;
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
        public TransactionsController(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
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
    }
}
