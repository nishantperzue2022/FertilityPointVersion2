using Microsoft.AspNetCore.Mvc;

namespace FertilityPoint.Web.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

     


    }
}
