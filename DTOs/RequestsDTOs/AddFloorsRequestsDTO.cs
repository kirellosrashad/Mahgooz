namespace STGeorgeReservation.DTOs.ResponseDTOs
{
    public class AddFloorsRequestsDTO
    {
      //  public Guid Id { get; set; }
        public string Name { get; set; }
       // public Guid BuildingId { get; set; }
        public List<AddRoomsRequestsDTO> Rooms { get; set; }
    }
}
