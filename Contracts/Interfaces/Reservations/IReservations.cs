using STGeorgeReservation.DTOs.ResponseDTOs;

namespace STGeorgeReservation.Contracts.Interfaces.Reservations
{
    public interface IReservations
    {

        Task<List<RoomAvailabilityResponseDTO>> CheckRoomAvailabilityAsync(Guid buildingId, Guid floorId, DateTime fromDate, DateTime toDate);
        Task AddReservationAsync(ReservationsResponseDTO reservation);

        Task<List<BuildingDTO>> GetAvailableRoomsAsync(DateTime fromDate, DateTime toDate);


    }
}
