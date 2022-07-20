using FertilityPoint.BLL.Repositories.TimeSlotModule;
using FertilityPoint.DAL.Modules;
using FertilityPoint.DTO.TimeSlotModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace FertilityPoint.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TimeSlotsController : Controller
    {
        private readonly ITimeSlotRepository timeSlotRepository;
        private readonly UserManager<AppUser> userManager;
        public TimeSlotsController(UserManager<AppUser> userManager, ITimeSlotRepository timeSlotRepository)
        {
            this.timeSlotRepository = timeSlotRepository;

            this.userManager = userManager;

        }
        public async Task<IActionResult> Index()
        {
            var timeSlots = await timeSlotRepository.GetAll();

            return View(timeSlots);
        }
        public IActionResult CreateSlots(DateTime AppointmentDate)
        {
            try
            {

                ViewBag.AppointmentDate = AppointmentDate.ToShortDateString();

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<IActionResult> Create(TimeSlotDTO timeSlotDTO)
        {
            try
            {

                var getSlots = await timeSlotRepository.GetAll();

                var isSlotExit = getSlots.Where(x => x.AppointmentDate == timeSlotDTO.AppointmentDate && x.FromTime == timeSlotDTO.FromTime && x.ToTime == timeSlotDTO.ToTime).Count();

                if (isSlotExit > 0)
                {
                    return Json(new { success = false, responseText = "The slot already exits" });

                }

                if (timeSlotDTO.FromTime == DateTime.Parse("01/01/0001 00:00:00"))
                {
                    return Json(new { success = false, responseText = "Invalid From Time" });
                }

                if (timeSlotDTO.ToTime == DateTime.Parse("01/01/0001 00:00:00"))
                {
                    return Json(new { success = false, responseText = "Invalid To Time" });
                }

                if (timeSlotDTO.AppointmentDate == DateTime.Parse("01/01/0001 00:00:00"))
                {
                    return Json(new { success = false, responseText = "Invalid Appointment Date" });
                }

                else
                {
                    var loggedInuser = await userManager.FindByEmailAsync(User.Identity.Name);

                    timeSlotDTO.CreateBy = loggedInuser.Id;

                    var result = await timeSlotRepository.Create(timeSlotDTO);

                    if (result != null)
                    {
                        return Json(new { success = true, responseText = "Record has been saved successfully" });

                    }
                    else
                    {
                        return Json(new { success = false, responseText = "Failed to save record" });
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new { success = false, responseText = "Something went wrong" });

            }
        }
        public async Task<IActionResult> Update(TimeSlotDTO timeSlotDTO)
        {
            try
            {
                var loggedInuser = await userManager.FindByEmailAsync(User.Identity.Name);

                timeSlotDTO.UpdatedBy = loggedInuser.Id;

                var results = await timeSlotRepository.Update(timeSlotDTO);

                if (results != null)
                {
                    return Json(new { success = true, responseText = "Record has been successfully updated" });
                }
                else
                {
                    return Json(new { success = false, responseText = "Failed to update record!" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new { success = false, responseText = "Something went wrong" });
            }
        }
        public async Task<ActionResult> Delete(Guid Id)
        {
            try
            {
                var results = await timeSlotRepository.Delete(Id);

                if (results == true)
                {

                    return Json(new { success = true, responseText = "Record has been deleted successfully" });
                }
                else
                {
                    return Json(new { success = false, responseText = "Unable to delete this record" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new { success = false, responseText = "Something went wrong" });
            }
        }
        public IActionResult GetById(Guid Id)
        {
            try
            {
                var timeslot = timeSlotRepository.GetById(Id);

                if (timeslot != null)
                {
                    TimeSlotDTO file = new TimeSlotDTO()
                    {
                        Id = timeslot.Id,

                        FromTime = timeslot.FromTime,

                        ToTime = timeslot.ToTime,

                        CreateBy = timeslot.CreateBy,

                        UpdatedBy = timeslot.UpdatedBy,
                    };

                    return Json(new { data = file });
                }
                return Json(new { data = false });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new { success = false, responseText = "Something went wrong" });
            }

        }

        public async Task<IActionResult> GetSlotsByDate()
        {
            var slots = (await timeSlotRepository.GetAll()).OrderBy(x => x.CreateDate).ToList();

            return Ok(slots);
        }

        public IActionResult Test()
        {


            return View();
        }

      
        public async Task<IActionResult> AjaxMethod()
        {
            var slots = (await timeSlotRepository.GetAll()).OrderBy(x => x.CreateDate).ToList();

            return Json(new {data=slots});
        }





    }
}
