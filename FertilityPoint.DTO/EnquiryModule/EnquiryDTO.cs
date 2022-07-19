using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FertilityPoint.DTO.EnquiryModule
{
    public class EnquiryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string FertilityEmail { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string PhoneNumber { get; set; }
        public byte Status { get; set; }
        public DateTime CreateDate { get; set; }
        public string NewCreateDate
        {
            get
            {
                return CreateDate.ToString("dd MMMM yyyy h:mm tt");


            }
        }
    }
}
