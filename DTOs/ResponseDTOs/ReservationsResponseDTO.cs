namespace STGeorgeReservation.DTOs.ResponseDTOs
{
    public class ReservationsResponseDTO
    {
       // public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ReservedBy { get; set; }
    }
}
