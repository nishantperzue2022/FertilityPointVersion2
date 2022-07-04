using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FertilityPoint.Controllers
{
    public class InfertilityController : Controller
    {
        public IActionResult Male()
        {
            return View();
        }

        public IActionResult Female()
        {
            return View();
        }
    }
}
