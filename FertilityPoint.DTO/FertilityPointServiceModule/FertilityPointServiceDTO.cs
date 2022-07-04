using System;
using System.Collections.Generic;
using System.Text;

namespace FertilityPoint.DTO.FertilityPointServiceModule
{
    public class FertilityPointServiceDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
