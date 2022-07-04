using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace FertilityPoint.DAL.Modules
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CreatedBy { get; set; }
        public Guid? SpecialityId { get; set; }
        public bool isActive { get; set; }       
        public DateTime CreateDate { get; set; }

    }
}
