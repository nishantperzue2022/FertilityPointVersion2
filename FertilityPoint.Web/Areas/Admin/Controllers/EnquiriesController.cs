using FertilityPoint.BLL.Repositories.EnquiryModule;
using Microsoft.AspNetCore.Mvc;

namespace FertilityPoint.Web.Areas.Admin.Controllers
{



    public class EnquiriesController : Controller
    {
        private readonly IEnquiryRepository enquiryRepository;
        public EnquiriesController(IEnquiryRepository enquiryRepository)
        {
            this.enquiryRepository = enquiryRepository;
        }
        public IActionResult Index()
        {
            var enquries = enquiryRepository.GetAll();

            return View(enquries);
        }
    }
}
