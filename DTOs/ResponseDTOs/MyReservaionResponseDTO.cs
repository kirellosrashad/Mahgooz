namespace STGeorgeReservation.DTOs.ResponseDTOs
{
    public class MyReservaionResponseDTO
    {
       // public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string RoomName { get; set; }
    }
}
