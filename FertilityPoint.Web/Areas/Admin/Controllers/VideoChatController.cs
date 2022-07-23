using FertilityPoint.BLL.Repositories.PatientModule;
using Microsoft.AspNetCore.Mvc;

namespace FertilityPoint.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VideoChatController : Controller
    {
        private readonly IPatientRepository patientRepository;
        public VideoChatController(IPatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;
        }

        public async Task<IActionResult> Index(Guid Id)
        {
            var patient = await patientRepository.GetById(Id);

            if (patient == null)
            {
                TempData["Error"] = "Something went wrong";

                return RedirectToAction("", "Appointments");
            }

            return View(patient);
        }

       
    }
}
