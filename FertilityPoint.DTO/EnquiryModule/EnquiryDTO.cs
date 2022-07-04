using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FertilityPoint.DTO.EnquiryModule
{
 public   class EnquiryDTO
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string FertilityEmail { get; set; }
        public string Email { get; set; }    
        public string Message { get; set; }
    }
}
