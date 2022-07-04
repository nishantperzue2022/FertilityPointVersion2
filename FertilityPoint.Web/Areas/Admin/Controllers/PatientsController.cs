using FertilityPoint.BLL.Repositories.PatientModule;
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
        public PatientsController(IPatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;
        }
        public async Task<IActionResult> Index()
        {
            var patients = await patientRepository.GetAll();

            return View(patients);
        }

        public async Task<IActionResult> ProfileAsync(Guid Id)
        {
            var patient = await patientRepository.GetById(Id);

            return View(patient);
        }
    }
}
