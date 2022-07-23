using FertilityPoint.BLL.Repositories.AppointmentModule;
using FertilityPoint.BLL.Repositories.PatientModule;
using FertilityPoint.DAL.Modules;
using FertilityPoint.DTO.VideoChatModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FertilityPoint.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VideoChatController : Controller
    {
        private readonly UserManager<AppUser> userManager;

        private readonly IAppointmentRepository appointmentRepository;
        public VideoChatController(UserManager<AppUser> userManager, IAppointmentRepository appointmentRepository)
        {
            this.appointmentRepository = appointmentRepository;

            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(Guid Id)
        {
            var patient = await appointmentRepository.GetById(Id);

            if (patient == null)
            {
                TempData["Error"] = "Something went wrong";

                return RedirectToAction("", "Appointments");
            }

            return View(patient);
        }

        public async Task<IActionResult> SaveRemark(RemarkDTO remarkDTO)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                remarkDTO.CreatedBy = user.Id;

                var result = await appointmentRepository.SaveVideoChatRemark(remarkDTO);

                if (result != null)
                {
                    return Json(new { success = true, responseText = "Remark has been saved successfully" });
                }
                else
                {
                    return Json(new { success = false, responseText = "Failed to save" });
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new { success = false, responseText = "Something went wrong" });
            }
        }
    }
}
