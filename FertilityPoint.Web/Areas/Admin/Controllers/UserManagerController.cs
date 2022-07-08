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

        private readonly SignInManager<AppUser> signInManager;
        public UserManagerController(SignInManager<AppUser> signInManager, IMailService mailService, IApplicationUserRepository applicationUserRepository, UserManager<AppUser> userManager, ISpecialityRepository specialityRepository)
        {
            this.specialityRepository = specialityRepository;

            this.userManager = userManager;

            this.applicationUserRepository = applicationUserRepository;

            this.mailService = mailService;

            this.signInManager = signInManager;
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
                if (Id == null)
                {
                    var loggedInUser = await userManager.FindByEmailAsync(User.Identity.Name);

                    var getuserDetails = await applicationUserRepository.GetById(loggedInUser.Id);

                    if (getuserDetails == null)
                    {
                        TempData["Error"] = "Something went wrong";

                        return RedirectToAction("Index", "UserManager");
                    }

                    return View(getuserDetails);
                }

                var user = await applicationUserRepository.GetById(Id);

                if (user == null)
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
        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordDTO updatePasswordDTO)
        {
            try
            {

                if (updatePasswordDTO.CurrentPassword == null)
                {
                    return Json(new { success = false, responseText = "Current password is a required field" });

                }

                if (updatePasswordDTO.NewPassword == null)
                {
                    return Json(new { success = false, responseText = "New password is a required field" });

                }

                if (updatePasswordDTO.ConfirmPassword == null)
                {
                    return Json(new { success = false, responseText = "Confirm password is a required field" });

                }

                if (updatePasswordDTO.ConfirmPassword != updatePasswordDTO.NewPassword)
                {
                    return Json(new { success = false, responseText = "Password and confirm password do not match" });

                }


                if (ModelState.IsValid)
                {
                    var user = await userManager.GetUserAsync(User);

                    if (user == null)
                    {
                        return RedirectToAction("/Account/Logout");
                    }

                    var result = await userManager.ChangePasswordAsync(user, updatePasswordDTO.CurrentPassword, updatePasswordDTO.NewPassword);

                    if (!result.Succeeded)
                    {

                        var validation = (result.Errors.FirstOrDefault().Description);

                        return Json(new { success = false, responseText = validation });

                    }

                    await signInManager.RefreshSignInAsync(user);

                    return Json(new { success = true, responseText = "Password has been changed successfully" });

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

            return View(updatePasswordDTO);
        }
        public async Task<ActionResult> DeactivateAccount(string Id)
        {
            try
            {
                var results = await applicationUserRepository.DisableAccount(Id);

                if (results == true)
                {

                    return Json(new { success = true, responseText = "Account has been successfully deactivated  " });
                }
                else
                {
                    return Json(new { success = false, responseText = "Unable to disabled account " });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<ActionResult> ActivateAccount(string Id)
        {
            try
            {
                var results = await applicationUserRepository.EnableAccount(Id);

                if (results == true)
                {
                    return Json(new { success = true, responseText = "Account has been successfully activated  " });
                }
                else
                {
                    return Json(new { success = false, responseText = "Unable to activate account " });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<IActionResult> GetUserById(string Id)
        {
            var data = await applicationUserRepository.GetById(Id);

            if (data != null)
            {
                ApplicationUserDTO file = new ApplicationUserDTO
                {
                    Id = data.Id,

                    Email = data.Email,

                    FirstName = data.FirstName,

                    LastName = data.LastName,

                    PhoneNumber = data.PhoneNumber,

                    CreateDate = data.CreateDate,

                    isActive = data.isActive,

                    CreatedByName = data.CreatedByName,

                    RoleName = data.RoleName,

                    RoleId = data.RoleId,

                };

                return Json(new { data = file });
            }
            else
            {
                return Json(new { data = false });
            }
        }
        public async Task<IActionResult> Update(ApplicationUserDTO applicationUserDTO)
        {
            try
            {
                var results = await applicationUserRepository.Update(applicationUserDTO);

                if (results != null)
                {
                    return Json(new { success = true, responseText = "Information has been updated successfully " });
                }
                else
                {
                    return Json(new { success = false, responseText = "Failed to update details" });
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
