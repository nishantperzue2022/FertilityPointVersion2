using System;
using System.Collections.Generic;
using System.Text;

namespace FertilityPoint.DTO.SpecialityModule
{
    public class SpecialityDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } 
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedName { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
    }
}
