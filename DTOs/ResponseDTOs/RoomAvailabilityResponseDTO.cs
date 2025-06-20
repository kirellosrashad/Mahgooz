namespace STGeorgeReservation.DTOs.ResponseDTOs
{
    public class RoomReservationDTO
    {
        public Guid ReservationId { get; set; }
        public Guid RoomId { get; set; }
        public string RoomName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string? ReservedBy { get; set; } // Optional
    }
}
