using FertilityPoint.DTO.ApplicationUserModule;
using FertilityPoint.DTO.AppointmentModule;
using FertilityPoint.DTO.PatientModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityPoint.DTO.DashboardModule
{
    public class DashboardDTO
    {
        public List<PatientDTO> PatientList { get; set; }
        public List<AppointmentDTO> AppointmentList { get; set; }
        public List<ApplicationUserDTO> SystemsUsers { get; set; }

        //public ImageDTO singleImageDTO { get; set; }
    }
}

