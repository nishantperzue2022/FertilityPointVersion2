using FertilityPoint.BLL.Repositories.EnquiryModule;
using FertilityPoint.DAL.Modules;
using FertilityPoint.DTO.EnquiryModule;
using FertilityPoint.Services.EmailModule;
using FertilityPoint.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace FertilityPoint.Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class EnquiriesController : Controller
    {
        private readonly IEnquiryRepository enquiryRepository;

        private readonly IMailService mailService;

        private readonly IHubContext<SignalrServer> signalrHub;

        private readonly UserManager<AppUser> userManager;

        public EnquiriesController(IMailService mailService, UserManager<AppUser> userManager, IEnquiryRepository enquiryRepository, IHubContext<SignalrServer> signalrHub)
        {
            this.enquiryRepository = enquiryRepository;

            this.signalrHub = signalrHub;

            this.userManager = userManager;

            this.mailService = mailService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                DateTime dateTime = DateTime.Now;

                var date = dateTime.ToString("dddd, dd MMMM yyyy");

                var time = dateTime.ToString("h:mm tt");

                ViewBag.Time = time + " " + date;

                var data = (await enquiryRepository.GetAll()).Where(x => x.Status == 0).Take(6);

                return View(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<IActionResult> ViewEnquiries()
        {

            var data = (await enquiryRepository.GetAll()).Where(x => x.Status == 0).Take(6);

            return View(data);

        }
        public async Task<IActionResult> GetEnquiries1()
        {
            var data = await enquiryRepository.GetAll();

            return Ok(data);

        }

        public async Task<IActionResult> SendMail(SentMailDTO sentMailDTO)
        {

            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                sentMailDTO.CreatedBy = user.Id;

                var result = await enquiryRepository.Reply(sentMailDTO);

                if (result != null)
                {
                    var sendMail = await mailService.ReplyMails(sentMailDTO);

                    return Json(new { success = true, responseText = "Message has been sent successfully" });
                }
                else
                {
                    return Json(new { success = false, responseText = "Failed to send message" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new { success = false, responseText = "Something went wrong" });
            }

        }
        public async Task<IActionResult> GetById(Guid Id)
        {
            try
            {
                var data = await enquiryRepository.GetById(Id);

                if (data != null)
                {
                    EnquiryDTO enquiryDTO = new EnquiryDTO();
                    {
                        enquiryDTO.Id = Id;

                        enquiryDTO.Name = data.Name;

                        enquiryDTO.Message = data.Message;

                        enquiryDTO.PhoneNumber = data.PhoneNumber;

                        enquiryDTO.CreateDate = data.CreateDate;

                        enquiryDTO.Status = data.Status;

                        enquiryDTO.Email = data.Email;
                    }

                    return Json(new { data = enquiryDTO });
                }

                return Json(new { data = false });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }
        public async Task<ActionResult> GetEnquiries()
        {
            try
            {
                var enquiries = (await enquiryRepository.GetAll()).Where(x => x.Status == 0).Take(6).OrderBy(x => x.CreateDate);

                return Json(enquiries.Select(x => new
                {
                    x.Id,

                    x.Name,

                    x.PhoneNumber,

                    x.Email,

                    x.NewCreateDate,


                }).ToList());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

    }
}
