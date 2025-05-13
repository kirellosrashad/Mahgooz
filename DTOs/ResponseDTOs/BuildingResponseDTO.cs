namespace STGeorgeReservation.DTOs.ResponseDTOs
{
    public class BuildingResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<FloorsResponseDTO> Floors { get; set; }
    }
}
