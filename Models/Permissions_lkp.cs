using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace STGeorgeReservation.Models
{
    public class Permissions_lkp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name {  get; set; } 
        public string NameEn {  get; set; } 
        public string NameAr {  get; set; } 
        public int CategoryId {  get; set; } 

    }
}
