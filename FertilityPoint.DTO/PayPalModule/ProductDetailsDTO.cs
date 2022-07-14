using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityPoint.DTO.PayPalModule
{
    public class ProductDetailsDTO
    {
        public List<ProductDTO> FindAll()
        {
            return new List<ProductDTO>

            {
                 new ProductDTO
                 {
                     Id="P0012",
                     Name="Appointment ",
                     Amount=1,
                     Quantity=1
                 },


            };
        }
    }
}
