using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FertilityPoint.DAL.Modules
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public byte Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime AppointmentDate { get; set; }
        public Guid PatientId { get; set; }
        public string? TransactionNumber { get; set; }
        public string? ApprovedBy { get; set; }          
        public Guid TimeSlotId { get; set; }
        public virtual TimeSlot TimeSlot { get; set; } = null!;

    }
}
