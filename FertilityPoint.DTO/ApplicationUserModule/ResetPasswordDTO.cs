using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
namespace FertilityPoint.DTO.ApplicationUserModule
{
    public class ResetPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "password and confirm password does not match")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }

    }
}
