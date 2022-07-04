using FertilityPoint.BLL.Repositories.ApplicationUserModule;
using FertilityPoint.BLL.Repositories.SpecialityModule;
using FertilityPoint.DAL.Modules;
using FertilityPoint.DTO.ApplicationUserModule;
using FertilityPoint.Web.Extensions;
using FertilityPoint.Services.EmailModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PasswordOptions = FertilityPoint.Web.Extensions.PasswordOptions;

namespace FertilityPoint.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserManagerController : Controller
    {
        private readonly ISpecialityRepository specialityRepository;

        private readonly IApplicationUserRepository applicationUserRepository;

        private readonly IMailService mailService;

        private readonly UserManager<AppUser> userManager;

        public UserManagerController(IMailService mailService, IApplicationUserRepository applicationUserRepository, UserManager<AppUser> userManager, ISpecialityRepository specialityRepository)
        {
            this.specialityRepository = specialityRepository;

            this.userManager = userManager;

            this.applicationUserRepository = applicationUserRepository;

            this.mailService = mailService;

        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.Speciality = await specialityRepository.GetAll();

                ViewBag.Roles = await applicationUserRepository.GetAll();

                var users = await applicationUserRepository.GetAllUsers();                

                return View(users);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }
        public async Task<IActionResult> Profile(string Id)
        {
            try
            {
                var user = await applicationUserRepository.GetById(Id);

                if(user == null)
                {
                    TempData["Error"] = "Something went wrong";

                    return RedirectToAction("Index", "UserManager");
                }

                return View(user);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(ApplicationUserDTO applicationUserDTO)
        {
            try
            {

                if (applicationUserDTO.RoleName == null)
                {
                    return Json(new { success = false, responseText = "Please select Role Name" });

                }
                var validateEmail = ValidateEmail.Validate(applicationUserDTO.Email);

                if (validateEmail.Success == false)
                {
                    return Json(new { success = false, responseText = "You have entered invalid email" });
                }

                var loggedInUser = await userManager.FindByEmailAsync(User.Identity.Name);

                applicationUserDTO.CreatedBy = loggedInUser.Id;

                string password = PasswordStore.GenerateRandomPassword(new PasswordOptions
                {
                    RequiredLength = 8,

                    RequireNonLetterOrDigit = true,

                    RequireDigit = true,

                    RequireLowercase = true,

                    RequireUppercase = true,

                    RequireNonAlphanumeric = true,

                    RequiredUniqueChars = 1
                });

                applicationUserDTO.Password = password;

                var user = new AppUser()
                {
                    UserName = applicationUserDTO.Email.ToLower(),

                    Email = applicationUserDTO.Email.ToLower(),

                    isActive = true,

                    PhoneNumber = applicationUserDTO.PhoneNumber,

                    FirstName = applicationUserDTO.FirstName.Substring(0, 1).ToUpper() + applicationUserDTO.FirstName.Substring(1).ToLower().Trim(),

                    LastName = applicationUserDTO.LastName.Substring(0, 1).ToUpper() + applicationUserDTO.LastName.Substring(1).ToLower().Trim(),

                    CreateDate = DateTime.Now,

                    CreatedBy = applicationUserDTO.CreatedBy,

                    SpecialityId = applicationUserDTO.SpecialityId,

                };

                var result = await userManager.CreateAsync(user, applicationUserDTO.Password);

                if (result.Succeeded == false)
                {
                    var error = result.Errors.FirstOrDefault();

                    return Json(new { success = false, responseText = error.Description });
                }

                if (result.Succeeded)
                {

                    // var sendmail = mailService.AccountEmailNotification(applicationUserDTO);

                    var createRole = await userManager.AddToRoleAsync(user, applicationUserDTO.RoleName);

                    return Json(new { success = true, responseText = "Account has been created successfully" });
                }

                foreach (var error in result.Errors)
                {
                    return Json(new { success = false, responseText = "Unable to update record report details" });

                }

                return View();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }
    }
}
