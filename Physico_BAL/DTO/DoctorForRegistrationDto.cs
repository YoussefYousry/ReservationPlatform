using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physico_BAL.DTO
{
    public class DoctorForRegistrationDto
    {
        public required string? Firstname { get; set; }
        public required string? Lastname { get; set; }
        [Required(ErrorMessage = "Username is a required field")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is a required field")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Email Address is required and should end with (@gmail.com) ")]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
