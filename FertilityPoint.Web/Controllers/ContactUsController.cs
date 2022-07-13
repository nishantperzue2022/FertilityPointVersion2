
using FertilityPoint.DTO.EnquiryModule;
using FertilityPoint.Web.Extensions;
using FertilityPoint.Services.EmailModule;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FertilityPoint.BLL.Repositories.EnquiryModule;

namespace FertilityPoint.Controllers
{

    public class ContactUsController : Controller
    {
        private readonly IMailService mailService;

        private readonly IEnquiryRepository enquiryRepository;
        public ContactUsController(IEnquiryRepository enquiryRepository,IMailService mailService)
        {
            this.mailService = mailService;

            this.enquiryRepository = enquiryRepository;

        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SendEmail(EnquiryDTO enquiryDTO)
        {
            try
            {
                if (enquiryDTO.Email == null || enquiryDTO.Email == string.Empty)
                {
                    return Json(new { success = false, responseText = "Email is a required field" });
                }

                var result = await enquiryRepository.Create(enquiryDTO);

                var validateEmail = ValidateEmail.Validate(enquiryDTO.Email);

                if (validateEmail.Success == true)
                {
                    var sendNotification = mailService.EnquiryNotification(enquiryDTO);

                    if (sendNotification == true)
                    {
                        return Json(new { success = true, responseText = "Your message has been sent successfully " });
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "Failed to send message" });
                    }
                }
                else
                {
                    return Json(new { success = false, responseText = "You have entered invalid email" });

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
