using Microsoft.AspNetCore.Mvc;

namespace FertilityPoint.Web.Controllers
{
    public class VideoChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
