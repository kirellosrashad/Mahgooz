namespace STGeorgeReservation.DTOs.ResponseDTOs
{
    public class AddBuildingRequestsDTO
    {
     //   public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<AddFloorsRequestsDTO> Floors { get; set; }
    }
}
