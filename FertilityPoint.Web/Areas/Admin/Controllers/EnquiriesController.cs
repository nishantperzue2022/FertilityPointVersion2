using FertilityPoint.BLL.Repositories.EnquiryModule;
using FertilityPoint.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace FertilityPoint.Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class EnquiriesController : Controller
    {
        private readonly IEnquiryRepository enquiryRepository;

        private readonly IHubContext<SignalrServer> signalrHub;

        public EnquiriesController(IEnquiryRepository enquiryRepository, IHubContext<SignalrServer> signalrHub)
        {
            this.enquiryRepository = enquiryRepository;

            this.signalrHub = signalrHub;
        }
        public IActionResult Index()
        {
            try
            {
                DateTime dateTime = DateTime.Now;

                var date = dateTime.ToString("dddd, dd MMMM yyyy");

                var time = dateTime.ToString("h:mm tt");

                ViewBag.Time = time + " " + date;

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }


        public async Task<IActionResult> GetEnquiries()
        {
          

            var data = await enquiryRepository.GetAll();

            await signalrHub.Clients.All.SendAsync("LoadEnquiries");

            return Ok(data);

        }




    }
}
