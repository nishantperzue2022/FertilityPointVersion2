using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FertilityPoint.DTO.ApplicationUserModule
{
    public class RegisterDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and Confirmation Password do not match")]
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }

        public string RoleId { get; set; }
        public string Id { get; set; }
        public string RoleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullNames { get; set; }
        public string Message { get; set; }
        public string LoginLink { get; set; }
        public string FullName => FirstName + " " + LastName;
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public string FullAddress { get; set; }
        public string Gender { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
    }
}
