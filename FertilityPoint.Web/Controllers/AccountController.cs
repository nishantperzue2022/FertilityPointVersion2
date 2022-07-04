using FertilityPoint.DAL.Modules;
using FertilityPoint.DTO.ApplicationUserModule;
using FertilityPoint.Web.Extensions;
using FertilityPoint.Services.EmailModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using System.Linq;
using PasswordOptions = FertilityPoint.Web.Extensions.PasswordOptions;

namespace FertilityPoint.Controllers
{

    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> signInManager;

        private readonly RoleManager<IdentityRole> roleManager;

        private readonly UserManager<AppUser> userManager;

        private readonly IConfiguration config;

        private readonly IMailService mailService;

        public AccountController(IMailService mailService, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, IConfiguration config)
        {
            this.signInManager = signInManager;

            this.roleManager = roleManager;

            this.userManager = userManager;

            this.config = config;

            this.mailService = mailService;

        }

        public IActionResult Index()
        {
            try
            {
                LoginDTO loginModel = new LoginDTO();

                return View(loginModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<IActionResult> Logout()
        {
            try
            {
                await signInManager.SignOutAsync();

                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public IActionResult Login()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
                string message;

                int responsetypeid;

                var user = await userManager.FindByEmailAsync(loginDTO.Email);

                if (user == null)
                {
                    message = "Invalid user account";

                    responsetypeid = 0;

                    return Json(new { message = message, status = responsetypeid });
                }

                if (user.isActive == false)
                {

                    message = "Your account has been de-activated,kindly contact system administrator";

                    responsetypeid = 0;

                    return Json(new { message = message, status = responsetypeid });
                }

                var result = await signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, loginDTO.RemeberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    var getUserRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();

                    message = "successful login";

                    responsetypeid = 1;

                    return Json(new { message = message, status = responsetypeid, role = getUserRole });

                }

                if (result.IsLockedOut)
                {
                    message = "This account has been locked out,please try again later";

                    responsetypeid = 0;

                    return Json(new { message = message, status = responsetypeid });

                }
                else
                {

                    message = "Wrong user name or password";

                    responsetypeid = 0;

                    return Json(new { message = message, status = responsetypeid });

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                var message = "Something went wrong";

                var responsetypeid = 0;

                return Json(new { message = message, status = responsetypeid });
            }

        }

        public IActionResult ResetPassword()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {

            try
            {
                if (token == null || email == null)
                {
                    ModelState.AddModelError("", "Invalid password reset token");
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
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await userManager.FindByEmailAsync(resetPasswordDTO.Email);

                    if (user != null)
                    {
                        var result = await userManager.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.Password);

                        if (result.Succeeded)
                        {
                            await signInManager.RefreshSignInAsync(user);

                            return View("Login", "Account");
                        }

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(resetPasswordDTO);

                    }
                    return View("ResetPasswordConfirmation");
                }
                return View(resetPasswordDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<IActionResult> SendPassword(ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(resetPasswordDTO.Email);

                if (user != null)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);

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

                    resetPasswordDTO.Token = token;

                    resetPasswordDTO.Password = password;

                    resetPasswordDTO.FullName = user.FirstName + " " + user.LastName;

                    resetPasswordDTO.PhoneNumber = user.PhoneNumber;

                    var result = await userManager.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.Password);

                    // var sms = mailService.PaswordResetSMS(resetPasswordDTO);

                    var sendmail = mailService.PasswordResetEmailNotification(resetPasswordDTO);

                    return Json(new { success = true, responseText = "If you have an account with us , we have sent a new  password  to " + resetPasswordDTO.Email + "" });

                }

                return Json(new { success = false, responseText = "You have entered an invalid account. " + resetPasswordDTO.Email + "does not exist" });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }

        public IActionResult ForgotPassword()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public IActionResult ResetPasswordConfirmation()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO)
        {
            try
            {
                try
                {
                    if (ModelState.IsValid)
                    {

                        try
                        {
                            var user = await userManager.FindByEmailAsync(forgotPasswordDTO.Email);

                            if (user != null)
                            {
                                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                                var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = forgotPasswordDTO.Email, token }, Request.Scheme);

                                forgotPasswordDTO.ResetLink = passwordResetLink;

                                forgotPasswordDTO.FullName = user.FirstName + " " + user.LastName;

                                // var sendEmail = SendEmailNotification(forgotPasswordDTO);

                                return Json(new { success = true, responseText = "If you have an account with us , we have sent a password resend link to " + forgotPasswordDTO.Email + "" });

                                //TempData["Info"] = "If you have an account with us , we have sent a password resend link to " + forgotPasswordDTO.Email + "";

                                //return RedirectToAction("Login", "Account");
                            }

                            return Json(new { success = true, responseText = "If you have an account with us , we have sent a password resend link to " + forgotPasswordDTO.Email + "" });


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);

                            return null;
                        }
                    }

                    return View(forgotPasswordDTO);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;

            }
        }
        public IActionResult Agents()
        {
            try
            {
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
