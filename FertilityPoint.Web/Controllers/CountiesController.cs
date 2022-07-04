using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FertilityPoint.BLL.Repositories.CountyModule;
using FertilityPoint.DTO.CountyModule;

using Microsoft.AspNetCore.Mvc;

namespace FertilityPoint.Controllers
{
    public class CountiesController : Controller
    {
        private readonly ICountyRepository countyRepository;
        public CountiesController(ICountyRepository countyRepository)
        {
            this.countyRepository = countyRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create(CountyDTO countyDTO)
        {
            try
            {
                countyDTO.Id = Guid.NewGuid();

                countyDTO.CreateDate = DateTime.Now;

                var results = await countyRepository.Create(countyDTO);

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
