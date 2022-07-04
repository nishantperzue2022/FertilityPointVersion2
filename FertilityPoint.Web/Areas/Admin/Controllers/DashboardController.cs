using FertilityPoint.BLL.Repositories.ApplicationUserModule;
using FertilityPoint.BLL.Repositories.AppointmentModule;
using FertilityPoint.BLL.Repositories.MpesaStkModule;
using FertilityPoint.BLL.Repositories.PatientModule;
using FertilityPoint.DTO.DashboardModule;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FertilityPoint.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly IAppointmentRepository appointmentRepository;

        private readonly IPatientRepository patientRepository;

        private readonly IPaymentRepository paymentRepository;

        private readonly IApplicationUserRepository applicationUserRepository;
        public DashboardController(
            IApplicationUserRepository applicationUserRepository,

            IPaymentRepository paymentRepository,

            IPatientRepository patientRepository,

            IAppointmentRepository appointmentRepository)
        {
            this.appointmentRepository = appointmentRepository;

            this.patientRepository = patientRepository;

            this.paymentRepository = paymentRepository;

            this.applicationUserRepository = applicationUserRepository;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var appointment = await appointmentRepository.GetAll();

                var patients = await patientRepository.GetAll();

                var revenue = paymentRepository.GetTotalRevenue();

                var users = await applicationUserRepository.GetAllUsers();

                ViewBag.Revenue = revenue;

                ViewBag.Appointment = appointment.Count();

                ViewBag.PendingAppointment = appointment.Where(x => x.Status == 0).Count();

                ViewBag.Patients = patients.Count();

                DashboardDTO data = new DashboardDTO()
                {

                };

                data.AppointmentList = appointment.Take(5).ToList();

                data.PatientList = patients.Take(5).OrderBy(x=>x.CreateDate).ToList();

                data.SystemsUsers = users.Where(x => x.RoleName == "Doctor").Take(5).ToList();

                return View(data);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
    }
}
