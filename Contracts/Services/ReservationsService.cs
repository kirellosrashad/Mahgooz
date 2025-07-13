using HRCom.Utilities.Services;
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

        public async Task<List<RoomReservationDTO>> GetFloorReservationsAfterYesterdayAsync(Guid RoomId)
        {
            var yesterday = DateTime.UtcNow.Date.AddDays(-1);

            var reservations = await _ApplicationDbContext.Reservations
                .Where(res => res.RoomId == RoomId && res.FromDate > yesterday)
                .Select(res => new RoomReservationDTO
                {
                    ReservationId = res.Id,
                    RoomId = res.RoomId,
                    RoomName = res.Room.Name,
                    FromDate = res.FromDate,
                    ToDate = res.ToDate,
                    ReservedBy = res.ReservedBy
                })
                .ToListAsync();

            return reservations;
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
                .Include(r => r.Reservations) // ✅ Ensure reservations are loaded
                .Include(r => r.Floor)
                    .ThenInclude(f => f.Building)
                .Where(r => !r.Reservations.Any(res =>
                    fromDate < res.ToDate && toDate > res.FromDate)) // ✅ Correct overlap logic
                .ToListAsync();

            // Hierarchical structure
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


        public async Task<List<MyReservaionResponseDTO>> GetReservationsByUserAsync(string UserId)
        {
            var reservations = await _ApplicationDbContext.Reservations
                .Include(r => r.Room)
                .Where(r => r.ReservedBy == UserId)
                .ToListAsync();

            return reservations.Select(r => new MyReservaionResponseDTO
            {
                RoomId = r.RoomId,
                FromDate = r.FromDate,
                ToDate = r.ToDate,
                RoomName = r.Room?.Name // Example additional data
            }).ToList();
        }


    }
}
