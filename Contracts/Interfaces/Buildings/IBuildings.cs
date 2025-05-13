using STGeorgeReservation.BaseTypes;
using STGeorgeReservation.DTOs.ResponseDTOs;

namespace STGeorgeReservation.Contracts.Interfaces.Buildings
{
    public interface IBuildings
    {
        Task<List<BuildingResponseDTO>> GetAllBuildingsAsync();
        Task<BuildingResponseDTO> GetBuildingByIdAsync(Guid id);
        Task<OperationResult<bool>> AddBuildingAsync(AddBuildingRequestsDTO model);

    }

}
