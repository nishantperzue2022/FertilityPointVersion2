using System;
using System.Collections.Generic;
using System.Text;
namespace FertilityPoint.DTO.MpesaStkModule
{
    public class MpesaExpressRequestDTO
    {
        public int BusinessShortCode { get; set; }

        public string Password { get; set; }

        public string Timestamp { get; set; }

        public string TransactionType { get; set; }

        public int Amount { get; set; }

        public string PartyA { get; set; }

        public int PartyB { get; set; }

        public string PhoneNumber { get; set; }

        public string CallBackURL { get; set; }

        public string AccountReference { get; set; }

        public string TransactionDesc { get; set; }

    }
}
