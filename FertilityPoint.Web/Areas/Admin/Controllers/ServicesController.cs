
using FertilityPoint.BLL.Repositories.ServiceModule;
using FertilityPoint.DAL.Modules;
using FertilityPoint.DTO.ServiceModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FertilityPoint.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServicesController : Controller
    {
        private readonly IServicesRepository servicesRepository;

        private readonly UserManager<AppUser> userManager;
        public ServicesController(UserManager<AppUser> userManager, IServicesRepository servicesRepository)
        {
            this.servicesRepository = servicesRepository;

            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var result = await servicesRepository.GetAll();

                return View(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new { success = false, responseText = "Something went wrong" });
            }
        }

        public async Task<IActionResult> Create(ServiceDTO serviceDTO)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                serviceDTO.CreatedBy = user.Id;

                var result = await servicesRepository.Create(serviceDTO);

                if (result != null)
                {
                    return Json(new { success = true, responseText = "Record has been saved successfully" });
                }
                else
                {
                    return Json(new { success = false, responseText = "Failed to save record" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new { success = false, responseText = "Something went wrong" });
            }
        }

        public async Task<IActionResult> Update(ServiceDTO serviceDTO)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                serviceDTO.UpdatedBy = user.Id;

                var results = await servicesRepository.Update(serviceDTO);

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
        public async Task<IActionResult> GetById(Guid Id)
        {
            try
            {
                var data = await servicesRepository.GetById(Id);

                if (data != null)
                {
                    ServiceDTO file = new ServiceDTO
                    {
                        Id = data.Id,

                        Name = data.Name,

                        Amount = data.Amount,

                        CreateDate = data.CreateDate,
                    };

                    return Json(new { data = file });
                }
                return Json(new { data = false });
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
                var results = await servicesRepository.Delete(Id);

                if (results == true)
                {

                    return Json(new { success = true, responseText = "Record has been deleted successfully" });
                }
                else
                {
                    return Json(new { success = false, responseText = "Unable to delete record " });
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
