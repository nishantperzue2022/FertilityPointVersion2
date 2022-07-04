using System;
using System.Collections.Generic;
using System.Text;

namespace FertilityPoint.DTO.CountyModule
{
    public class CountyDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public DateTime CreateDate;
    }
}
