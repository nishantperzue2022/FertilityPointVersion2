using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityPoint.DAL.Modules
{
    public partial class TimeSlot
    {
        public Guid Id { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public string CreateBy { get; set; }
        public string UpdatedBy { get; set; }
        public byte IsBooked { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
