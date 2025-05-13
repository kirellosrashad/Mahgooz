namespace STGeorgeReservation.DTOs.ResponseDTOs
{
    public class RoomDTO
    {
        public Guid RoomId { get; set; }
        public string RoomName { get; set; }
        public int Capacity { get; set; }
    }

    public class FloorDTO
    {
        public Guid FloorId { get; set; }
        public string FloorName { get; set; }
        public List<RoomDTO> Rooms { get; set; } = new List<RoomDTO>();
    }

    public class BuildingDTO
    {
        public Guid BuildingId { get; set; }
        public string BuildingName { get; set; }
        public List<FloorDTO> Floors { get; set; } = new List<FloorDTO>();
    }
}
