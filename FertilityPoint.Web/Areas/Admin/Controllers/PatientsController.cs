using FertilityPoint.BLL.Repositories.PatientModule;
using FertilityPoint.DAL.Modules;
using FertilityPoint.DTO.PatientModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FertilityPoint.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PatientsController : Controller
    {
        private readonly IPatientRepository patientRepository;

        private readonly UserManager<AppUser> userManager;
        public PatientsController(UserManager<AppUser> userManager, IPatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;

            this.userManager = userManager;

        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var patients = await patientRepository.GetAll();

                return View(patients);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<IActionResult> Profile(Guid Id)
        {
            try
            {
                var patient = await patientRepository.GetById(Id);

                return View(patient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<ActionResult> Update(PatientDTO patientDTO)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                patientDTO.UpdatedBy = user.Id;

                var results = await patientRepository.Update(patientDTO);

                if (results != null)
                {
                    return Json(new { success = true, responseText = "Record has been successfully updated" });
                }
                else
                {
                    return Json(new { success = false, responseText = "Unable to update patient information " });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new { success = false, responseText = ex.Message });
            }
        }
        public async Task<ActionResult> Delete(Guid Id)
        {
            try
            {
                var results = await patientRepository.Delete(Id);

                if (results == true)
                {

                    return Json(new { success = true, responseText = "Patient has been deleted successfully" });
                }
                else
                {
                    return Json(new { success = false, responseText = "Unable to delete patient " });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new { success = false, responseText = ex.Message });
            }
        }
        public async Task<IActionResult> GetById(Guid Id)
        {
            try
            {
                var data = await patientRepository.GetById(Id);

                if (data != null)
                {
                    PatientDTO result = new PatientDTO();
                    {
                        result.Id = data.Id;

                        result.FirstName = data.FirstName;

                        result.LastName = data.LastName;

                        result.Email = data.Email;

                        result.PhoneNumber = data.PhoneNumber;

                    }

                    return Json(new { data = result });

                }

                return Json(new { data = false });
            }
            catch (Exception ex)
            {
                Console.WriteLine();

                return Json(new { success = false, responseText = ex.Message });
            }
        }


    }
}
