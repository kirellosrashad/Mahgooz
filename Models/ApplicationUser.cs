using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace STGeorgeReservation.Models
{
    public class ApplicationUser : IdentityUser
    {
        [System.ComponentModel.DataAnnotations.Required, MaxLength(50)]
        public string  FirstName { get; set; }

        [System.ComponentModel.DataAnnotations.Required, MaxLength(50)]


        public string  LastName { get; set; }

        //public string? OtpCode  { get; set; }
        //public bool? IsConfirmedOtp { get; set; }
        //public DateTime? OtpGenerationDate { get; set; }
        //public string? LoginOtpCode { get; set; }
        //public DateTime? LoginOtpGenerationDate { get; set; }
        public bool? IsSuperAdmin { get; set; }

    }
}
