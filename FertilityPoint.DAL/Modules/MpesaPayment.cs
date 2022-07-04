using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityPoint.DAL.Modules
{
    public partial class MpesaPayment
    {
        public int Id { get; set; }
        public string MerchantRequestID { get; set; }
        public string CheckoutRequestID { get; set; }
        public int? ResultCode { get; set; }
        public string ResultDesc { get; set; }
        public decimal Amount { get; set; }
        public string TransactionNumber { get; set; }
        public decimal? Balance { get; set; }
        public string TransactionDate { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte IsPaymentUsed { get; set; }
        public string ReceiptNo { get; set; }

    }
}
