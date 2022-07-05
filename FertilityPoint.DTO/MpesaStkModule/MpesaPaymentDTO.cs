using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityPoint.DTO.MpesaStkModule
{
    public class MpesaPaymentDTO
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
        public string FullName { get; set; }      
        public string ReceiptNo { get; set; }
        public byte IsPaymentUsed { get; set; }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string BusinessShortCode { get; set; }
        public string BillRefNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string OrgAccountBalance { get; set; }
        public string TransactionType { get; set; }
        public string ThirdPartyTransID { get; set; }

    }
}
