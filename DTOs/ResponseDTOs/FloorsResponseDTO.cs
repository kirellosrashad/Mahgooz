namespace STGeorgeReservation.DTOs.ResponseDTOs
{
    public class FloorsResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
      //  public Guid BuildingId { get; set; }
        public List<RoomsResponseDTO> Rooms { get; set; }
    }
}
