using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityPoint.DAL.Modules
{
    public partial class SentMail
    {
        public Guid Id { get; set; }
        public Guid EnquiryId { get; set; }
        public string? Message { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
