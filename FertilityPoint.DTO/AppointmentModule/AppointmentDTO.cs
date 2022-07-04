using System;
using System.Collections.Generic;
using System.Text;
namespace FertilityPoint.DTO.AppointmentModule
{
    public class AppointmentDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => FirstName + " " + LastName;
        public string Email { get; set; }
        public string FertilityEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string PaidByNumber { get; set; }
        public string CountryCode { get; set; }
        public byte Status { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid PatientId { get; set; }
        public Guid TimeId { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string NewAppointmentDate { get { return AppointmentDate.ToShortDateString(); } }
        public string TransactionNumber { get; set; }
        public string TimeSlot { get; set; }

        //public string TimeSlot => FromTime.ToString("h:mm tt") + " - " + ToTime.ToString("h:mm tt");            

        public string TransactionDate { get; set; }
        public string ReceiptURL { get; set; }
        public decimal Amount { get; set; }
        public string ApprovedBy { get; set; }
        public string ReceiptNo { get; set; }
        public string NewAmount { get { return Amount.ToString("N"); } }

    }
}
