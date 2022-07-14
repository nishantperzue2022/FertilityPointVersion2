using FertilityPoint.BLL.Repositories.AppointmentModule;
using FertilityPoint.DAL.Modules;
using FertilityPoint.DTO.AppointmentModule;
using FertilityPoint.Services.EmailModule;
using FertilityPoint.Services.SMSModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FertilityPoint.Web.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentRepository appointmentRepository;

        private readonly IMessagingService messagingService;

        private readonly IMailService mailService;

        private readonly UserManager<AppUser> userManager;
        public AppointmentsController(IMailService mailService, UserManager<AppUser> userManager, IMessagingService messagingService, IAppointmentRepository appointmentRepository)
        {
            this.appointmentRepository = appointmentRepository;

            this.messagingService = messagingService;

            this.userManager = userManager;

            this.mailService = mailService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var appointments = (await appointmentRepository.GetAll()).Where(x => x.Status == 0).OrderBy(x => x.CreateDate);

                return View(appointments);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<IActionResult> GetById(Guid Id)
        {
            try
            {
                var appointment = await appointmentRepository.GetById(Id);

                if (appointment != null)
                {
                    var file = new AppointmentDTO
                    {
                        Id = appointment.Id,

                        Status = appointment.Status,

                        CreateDate = appointment.CreateDate,

                        AppointmentDate = appointment.AppointmentDate,

                        FirstName = appointment.FirstName,

                        PhoneNumber = appointment.PhoneNumber,

                        Email = appointment.Email,

                        LastName = appointment.LastName,

                        TimeSlotId = appointment.TimeSlotId,

                        TimeSlot = appointment.TimeSlot,

                    };

                    return Json(new { data = file });
                }

                return Json(new { data = false });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }

        }
        public async Task<IActionResult> ApproveAppointment(AppointmentDTO appointmentDTO)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                appointmentDTO.ApprovedBy = user.Id;

                var result = await appointmentRepository.ApproveAppointment(appointmentDTO);

                if (result == true)
                {
                    var get_appointment = (await appointmentRepository.GetById(appointmentDTO.Id));

                    var appointment = new AppointmentDTO()
                    {
                        FirstName = get_appointment.FirstName,

                        PhoneNumber = get_appointment.PhoneNumber,

                        AppointmentDate = get_appointment.AppointmentDate,

                        Email = get_appointment.Email,

                        CreateDate = get_appointment.CreateDate,

                        FromTime = get_appointment.FromTime,

                        ToTime = get_appointment.ToTime,

                        TimeSlot = get_appointment.FromTime.ToString("h:mm tt") + " - " + get_appointment.ToTime.ToString("h:mm tt"),
                    };

                    var sendSMS = messagingService.ApprovalNotificationSMS(appointment);

                    var sendMail = mailService.AppointmentApprovalNotification(appointment);

                    return Json(new { success = true, responseText = "Appointment has been successfully approved" });
                }
                else
                {
                    return Json(new { success = false, responseText = "Appointment has not been  approved" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }
        public async Task<ActionResult> Delete(Guid Id)
        {
            try
            {
                var results = await appointmentRepository.Delete(Id);

                if (results == true)
                {

                    return Json(new { success = true, responseText = "Appointment has been deleted successfully" });
                }
                else
                {
                    return Json(new { success = false, responseText = "Unable to delete appointment " });
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
