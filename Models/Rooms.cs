using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using STGeorgeReservation.BaseTypes;

namespace STGeorgeReservation.Models
{
    public class Rooms : MetaDataBaseModel<Guid>
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public int Capacity { get; set; }

        [ForeignKey("Floor")]
        public Guid FloorId { get; set; }

        // Navigation Properties
        public virtual Floors Floor { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
