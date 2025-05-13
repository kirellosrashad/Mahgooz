using Microsoft.EntityFrameworkCore;
using STGeorgeReservation.BaseTypes;
using STGeorgeReservation.Contracts.Interfaces.Buildings;
using STGeorgeReservation.Data;
using STGeorgeReservation.DTOs.ResponseDTOs;
using STGeorgeReservation.Models;
using System.Net;

namespace STGeorgeReservation.Contracts.Services
{
    public class BuildingsService : IBuildings
    {

        private readonly ApplicationDbContext _ApplicationDbContext;

        public BuildingsService(ApplicationDbContext context)
        {
            _ApplicationDbContext = context;
        }
        public async Task<List<BuildingResponseDTO>> GetAllBuildingsAsync()
        {
            return await _ApplicationDbContext.Buildings
                .Include(b => b.Floors)
                .ThenInclude(f => f.Rooms)
                .Select(b => new BuildingResponseDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    Address = b.Address,
                    Floors = b.Floors.Select(f => new FloorsResponseDTO
                    {
                        Id = f.Id,
                        Name = f.Name,
                      //  BuildingId = f.BuildingId,
                        Rooms = f.Rooms.Select(r => new RoomsResponseDTO
                        {
                            Id = r.Id,
                            Name = r.Name,
                            Capacity = r.Capacity,
                           // FloorId = r.FloorId
                        }).ToList()
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<BuildingResponseDTO> GetBuildingByIdAsync(Guid id)
        {
            var building = await _ApplicationDbContext.Buildings
                .Include(b => b.Floors)
                .ThenInclude(f => f.Rooms)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (building == null) return null;

            return new BuildingResponseDTO
            {
                 Id = building.Id,
                Name = building.Name,
                Address = building.Address,
                Floors = building.Floors.Select(f => new FloorsResponseDTO
                {
                    Id = f.Id,
                    Name = f.Name,
                  //  BuildingId = f.BuildingId,
                    Rooms = f.Rooms.Select(r => new RoomsResponseDTO
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Capacity = r.Capacity,
                      //  FloorId = r.FloorId
                    }).ToList()
                }).ToList()
            };
        }




        public async Task<OperationResult<bool>> AddBuildingAsync(AddBuildingRequestsDTO building)
        {
            OperationResult<bool> result = new OperationResult<bool>();

            // Create the new Building entity
            var newBuilding = new Buildings
            {
                Name = building.Name,
                Address = building.Address,
                Floors = new List<Floors>()
            };

            // Add the Building entity to the context
            _ApplicationDbContext.Buildings.Add(newBuilding);

            // Save changes to generate the BuildingId
            await _ApplicationDbContext.SaveChangesAsync();

            // Now, after the Building is saved and we have the new BuildingId
            // Add Floors and Rooms and set the BuildingId and FloorId
            foreach (var floor in building.Floors)
            {
                var newFloor = new Floors
                {
                    Name = floor.Name,
                    BuildingId = newBuilding.Id, // Set the BuildingId for this Floor
                    Rooms = new List<Rooms>()
                };

                _ApplicationDbContext.Floors.Add(newFloor);

                // Save the Floor to generate the FloorId
                await _ApplicationDbContext.SaveChangesAsync();

                // After the Floor is saved and we have the new FloorId, set it for the Rooms
                foreach (var room in floor.Rooms)
                {
                    var newRoom = new Rooms
                    {
                        Name = room.Name,
                        Capacity = room.Capacity,
                        FloorId = newFloor.Id // Set the FloorId for this Room
                    };

                    _ApplicationDbContext.Rooms.Add(newRoom);
                }

                // Save all Rooms in the context
                await _ApplicationDbContext.SaveChangesAsync();
            }

            // Final save if any changes are pending
            await _ApplicationDbContext.SaveChangesAsync();

            result.StatusCode = HttpStatusCode.OK;
            result.SuccessMessageKey = "Building added successfully along with its floors and rooms";

            return result;
        }


       
    }
}
