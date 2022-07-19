using Microsoft.AspNetCore.Mvc;

namespace FertilityPoint.Web.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
