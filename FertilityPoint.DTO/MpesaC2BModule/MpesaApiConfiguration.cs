using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityPoint.DTO.MpesaC2BModule
{
    public class MpesaApiConfiguration
    {
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string PassKey { get; set; }
        public string ConfirmationUrl { get; set; }
        public string ValidationUrl { get; set; }
        public string CallBackUrl { get; set; }
        public string LNMOshortCode { get; set; }
        public string C2BShortCode { get; set; } = "174379";
        public string C2BMSISDNTest { get; set; } = "254704509484";
    }
}
