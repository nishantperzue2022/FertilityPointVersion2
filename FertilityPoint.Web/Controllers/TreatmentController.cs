using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FertilityPoint.Controllers
{
    public class TreatmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        } 
        public IActionResult IUI()
        {
            return View();
        }  
        
        public IActionResult IVF()
        {
            return View();
        }
        
        public IActionResult ICSI()
        {
            return View();
        }

        public IActionResult Surrogacy()
        {
            return View();
        } 
        
        public IActionResult EggVitrification()
        {
            return View();
        }

        public IActionResult AssistedHatching()
        {
            return View();
        }

        public IActionResult BlastocystCulture()
        {
            return View();
        }  
        
        public IActionResult Laparoscopy()
        {
            return View();
        }
        
        public IActionResult OperativeHysteroscopy()
        {
            return View();
        }   
        
        public IActionResult MaleInfertility()
        {
            return View();
        } 
        
        public IActionResult FrozenEmbryoTransfer()
        {
            return View();
        }
    }
}
