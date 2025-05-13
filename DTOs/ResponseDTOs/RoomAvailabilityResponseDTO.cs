namespace STGeorgeReservation.DTOs.ResponseDTOs
{
    public class RoomAvailabilityResponseDTO
    {
        public Guid RoomId { get; set; }
        public string RoomName { get; set; }
        public bool IsAvailable { get; set; }
    }
}
