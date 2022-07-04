using FertilityPoint.BLL.Repositories.TimeSlotModule;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FertilityPoint.Controllers
{
    

    public class TestCheckBoxController : Controller
    {
        private readonly ITimeSlotRepository timeSlotRepository;

        public TestCheckBoxController(ITimeSlotRepository timeSlotRepository)
        {
            this.timeSlotRepository = timeSlotRepository;
        }
        public async Task<IActionResult> Index()
        {
            var timeslot = (await timeSlotRepository.GetAll()).Where(x => x.IsBooked == 0).OrderBy(x => x.TimeSlot).ToList();

            return View(timeslot);
        }  
        public async Task<IActionResult> GetSlots1()
        {
            var timeslot = (await timeSlotRepository.GetAll()).Where(x => x.IsBooked == 0).OrderBy(x => x.TimeSlot).ToList();

            return Json(new { data = timeslot });
        }


        public async Task<IActionResult> GetSlots()
        {
            var timeslot = (await timeSlotRepository.GetAll()).Where(x => x.IsBooked == 0).OrderBy(x => x.TimeSlot).ToList();

            return Json(timeslot.Select(x => new
            {
                SlotId = x.Id,

                SlotName = x.TimeSlot

            }));

        }
    }
}
