using System.ComponentModel.DataAnnotations;

namespace STGeorgeReservation.Models
{
    public class RegisterModel
    {
        [Required,StringLength(100)]
        public string FirstName { get; set; }
        
        
        [Required,StringLength(100)]
        public string LastName { get; set; } 
        
        
        [Required,StringLength(50)]
        public string UserName { get; set; } 
        
        
        [Required,StringLength(128)]
        public string Email { get; set; }    

        public bool? is_super_admin { get; set; }
        public string EncryptedPassword { get; set; } // Store the password securely

        public List<string> RolesId { get; set; }
    }
}
