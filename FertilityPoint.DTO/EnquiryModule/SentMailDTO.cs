using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityPoint.DTO.EnquiryModule
{
    public class SentMailDTO
    {
        public Guid Id { get; set; }
        public Guid EnquiryId { get; set; }
        public string Message { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
