using STGeorgeReservation.DTOs.ResponseDTOs;

namespace STGeorgeReservation.Contracts.Interfaces.Reservations
{
    public interface IReservations
    {

        Task<List<RoomReservationDTO>> GetFloorReservationsAfterYesterdayAsync(Guid RoomId);
        Task AddReservationAsync(ReservationsResponseDTO reservation);

        Task<List<BuildingDTO>> GetAvailableRoomsAsync(DateTime fromDate, DateTime toDate);


    }
}
