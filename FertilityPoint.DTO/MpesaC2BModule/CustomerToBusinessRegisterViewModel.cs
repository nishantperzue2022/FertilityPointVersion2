
using FertilityPoint.DTO.Enumurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityPoint.DTO.MpesaC2BModule
{
    public class CustomerToBusinessRegisterViewModel
    {
        public CustomerToBusinessResponseType customerToBusinessResponse { get; set; }
        public string ConfirmationUrl { get; set; }
        public string ValidationUrl { get; set; }
    }
}
