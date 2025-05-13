using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using STGeorgeReservation.BaseTypes;

namespace STGeorgeReservation.Models
{
    public class Buildings : MetaDataBaseModel<Guid>
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Address { get; set; }

        // Navigation Property
        public virtual ICollection<Floors> Floors { get; set; }
    }
}
