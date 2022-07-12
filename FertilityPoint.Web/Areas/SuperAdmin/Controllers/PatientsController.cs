using FertilityPoint.BLL.Repositories.PatientModule;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FertilityPoint.Web.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    public class PatientsController : Controller
    {
        private readonly IPatientRepository patientRepository;
        public PatientsController(IPatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;
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

                return null;
            }
        }

        public async Task<IActionResult> Profile(Guid Id)
        {
            var patient = await patientRepository.GetById(Id);

            return View(patient);
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

                return null;
            }
        }
    }
}
