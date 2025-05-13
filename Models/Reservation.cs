using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using STGeorgeReservation.BaseTypes;

namespace STGeorgeReservation.Models
{
    public class Reservation : MetaDataBaseModel<Guid>
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey("Room")]
        public Guid RoomId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        // Additional Fields
        public string ReservedBy { get; set; } // e.g., Customer name

        // Navigation Properties
        public virtual Rooms Room { get; set; }
    }
}
