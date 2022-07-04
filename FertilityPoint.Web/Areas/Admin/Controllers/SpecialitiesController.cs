using FertilityPoint.BLL.Repositories.SpecialityModule;
using FertilityPoint.DAL.Modules;
using FertilityPoint.DTO.SpecialityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FertilityPoint.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialitiesController : Controller
    {
        private readonly ISpecialityRepository specialityRepository;

        private readonly UserManager<AppUser> userManager;
        public SpecialitiesController(UserManager<AppUser> userManager, ISpecialityRepository specialityRepository)
        {
            this.specialityRepository = specialityRepository;

            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var speciality = await specialityRepository.GetAll();

            return View(speciality);
        }

        public async Task<IActionResult> Create(SpecialityDTO specialityDTO)
        {
            try
            {
                var loggedInuser = await userManager.FindByEmailAsync(User.Identity.Name);

                specialityDTO.CreatedBy = loggedInuser.Id;

                var result = await specialityRepository.Create(specialityDTO);

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


        public async Task<IActionResult> Update(SpecialityDTO specialityDTO)
        {
            try
            {
                var loggedInuser = await userManager.FindByEmailAsync(User.Identity.Name);

                specialityDTO.UpdatedBy = loggedInuser.Id;

                var results = await specialityRepository.Update(specialityDTO);

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
                var data = await specialityRepository.GetById(Id);

                if (data != null)
                {
                    SpecialityDTO file = new SpecialityDTO
                    {
                        Id = data.Id,

                        Name = data.Name,

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
                var results = await specialityRepository.Delete(Id);

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
