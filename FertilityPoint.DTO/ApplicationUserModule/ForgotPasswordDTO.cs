using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FertilityPoint.DTO.ApplicationUserModule
{
    public class ForgotPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string ResetLink { get; set; }
        public string FullName { get; set; }
    }
}
