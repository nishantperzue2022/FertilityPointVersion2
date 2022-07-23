using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityPoint.DTO.VideoChatModule
{
    public class RemarkDTO
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid AppointmentId { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
