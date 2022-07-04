using FertilityPoint.BLL.Repositories.CountyModule;
using FertilityPoint.BLL.Repositories.SubCountyModule;
using FertilityPoint.DTO.SubCountyModule;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FertilityPoint.Controllers
{
    public class SubCountyController : Controller
    {
        private readonly ICountyRepository countyRepository;

        private readonly ISubCountyRepository subCountyRepository;
        public SubCountyController(ISubCountyRepository subCountyRepository, ICountyRepository countyRepository)
        {
            this.countyRepository = countyRepository;

            this.subCountyRepository = subCountyRepository;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.Counties = (await countyRepository.GetAll()).OrderBy(x => x.Name);

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;

            }
        }

        public async Task<IActionResult> Create(SubCountyDTO subCountyDTO)
        {
            try
            {
                var isExist = (await subCountyRepository.GetAll()).Where(x => x.Name == subCountyDTO.Name).Count();

                if (isExist > 0)
                {
                    return Json(new { success = false, responseText = "Record exist in the system" });

                }
                var results = await subCountyRepository.Create(subCountyDTO);

                if (results != null)
                {
                    return Json(new { success = true, responseText = "Record has been successfully submitted" });
                }
                else
                {
                    return Json(new { success = false, responseText = "Failed to submitted Record" });
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
