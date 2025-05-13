using HRCom.Utilities.Services;
using Microsoft.EntityFrameworkCore;
using STGeorgeReservation.Contracts.Interfaces.Reservations;
using STGeorgeReservation.Data;
using STGeorgeReservation.DTOs.ResponseDTOs;
using STGeorgeReservation.Models;

namespace STGeorgeReservation.Contracts.Services
{
    public class ReservationsService : IReservations
    {
        private readonly ApplicationDbContext _ApplicationDbContext;
        private readonly IUserDataProvider _UserDataProvider;

        public ReservationsService(ApplicationDbContext context , IUserDataProvider UserDataProvider)
        {
            _ApplicationDbContext = context;
            _UserDataProvider = UserDataProvider;

        }

        public async Task<List<RoomAvailabilityResponseDTO>> CheckRoomAvailabilityAsync(Guid buildingId, Guid floorId, DateTime fromDate, DateTime toDate )
        {
            var rooms = await _ApplicationDbContext.Rooms
                .Where(r => r.FloorId == floorId && r.Floor.BuildingId == buildingId)
                .Select(r => new RoomAvailabilityResponseDTO
                {
                    RoomId = r.Id,
                    RoomName = r.Name,
                    IsAvailable = !r.Reservations.Any(res => res.FromDate < toDate && res.ToDate > fromDate)
                }).ToListAsync();

            return rooms;
        }

        public async Task AddReservationAsync(ReservationsResponseDTO reservation)
        {
            var newReservation = new Reservation
            {
                RoomId = reservation.RoomId,
                FromDate = reservation.FromDate,
                ToDate = reservation.ToDate,
                ReservedBy = _UserDataProvider.GetUserId().ToString()
            };

            _ApplicationDbContext.Reservations.Add(newReservation);
            await _ApplicationDbContext.SaveChangesAsync();
        }

        public async Task<List<BuildingDTO>> GetAvailableRoomsAsync(DateTime fromDate, DateTime toDate)
        {
            var rooms = await _ApplicationDbContext.Rooms
                .Include(r => r.Floor)
                    .ThenInclude(f => f.Building)
                .Where(r => !r.Reservations.Any(res =>
                    (fromDate < res.ToDate && toDate > res.FromDate))) // Filter out booked rooms
                .ToListAsync();

            // Organize results hierarchically
            var buildings = rooms.GroupBy(r => r.Floor.Building)
                .Select(buildingGroup => new BuildingDTO
                {
                    BuildingId = buildingGroup.Key.Id,
                    BuildingName = buildingGroup.Key.Name,
                    Floors = buildingGroup.GroupBy(r => r.Floor)
                        .Select(floorGroup => new FloorDTO
                        {
                            FloorId = floorGroup.Key.Id,
                            FloorName = floorGroup.Key.Name,
                            Rooms = floorGroup.Select(r => new RoomDTO
                            {
                                RoomId = r.Id,
                                RoomName = r.Name,
                                Capacity = r.Capacity
                            }).ToList()
                        }).ToList()
                }).ToList();

            return buildings;
        }



    }
}
