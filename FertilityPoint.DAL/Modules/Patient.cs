using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityPoint.DAL.Modules
{
    public partial class Patient
    {
        public Guid  Id { get; set; }
        public string  FirstName { get; set; }
        public string  LastName { get; set; }
        public string  Email { get; set; }
        public string  PhoneNumber { get; set; }
        public DateTime  CreateDate { get; set; }
    }
}
