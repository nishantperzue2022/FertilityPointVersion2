using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FertilityPoint.Controllers
{
    public class Sperm_DonationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
