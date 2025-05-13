namespace STGeorgeReservation.DTOs.ResponseDTOs
{
    public class RoomsResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        //public Guid FloorId { get; set; }
    }
}
