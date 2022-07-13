using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityPoint.DAL.Modules
{
    public partial class Enquiry
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Subject { get; set; }
        public string? FertilityEmail { get; set; }
        public string? Email { get; set; }
        public string? Message { get; set; }
        public string? PhoneNumber { get; set; }
        public byte Status { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
