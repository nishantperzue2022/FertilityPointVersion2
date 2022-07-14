using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityPoint.DAL.Modules
{
    public partial class PaypalPayment
    {
        public Guid Id { get; set; }
        public string? TransactionId { get; set; }
        public string? PayerId { get; set; }
        public string? PayerFirstName { get; set; }
        public string? PayerLastName { get; set; }
        public string? PayerEmail { get; set; }
        public string? PayerCountryCode { get; set; }
        public string? Status { get; set; }
        public string? Intent { get; set; }
        public string? Cart { get; set; }
        public string? RecipientCity { get; set; }
        public string? RecipientCountryCode { get; set; }
        public string? Line1 { get; set; }
        public string? RecipientPostalCode { get; set; }
        public string? RecipientName { get; set; }
        public string? RecipientEmail { get; set; }
        public string? MerchantId { get; set; }
        public string? State { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Currency { get; set; }
        public double? AmountPaid { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
