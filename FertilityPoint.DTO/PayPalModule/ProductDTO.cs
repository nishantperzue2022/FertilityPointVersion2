﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityPoint.DTO.PayPalModule
{
    public class ProductDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public int Quantity { get; set; }

    }
}
