using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using STGeorgeReservation.BaseTypes;

namespace STGeorgeReservation.Models
{
    public class Floors : MetaDataBaseModel<Guid>
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        [ForeignKey("Building")]
        public Guid BuildingId { get; set; }

        // Navigation Properties
        public virtual Buildings Building { get; set; }
        public virtual ICollection<Rooms> Rooms { get; set; }
    }
}
