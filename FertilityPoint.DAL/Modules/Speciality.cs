using System;
using System.Collections.Generic;
using System.Text;

namespace FertilityPoint.DAL.Modules
{
    public partial class Speciality
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
