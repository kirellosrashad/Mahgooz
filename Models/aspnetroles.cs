using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace STGeorgeReservation.Models
{
    public class aspnetroles : IdentityRole
    {
      public string? NameAr {  get; set; }

    }
}
