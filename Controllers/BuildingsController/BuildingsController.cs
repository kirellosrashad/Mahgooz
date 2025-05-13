using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STGeorgeReservation.BaseTypes;
using STGeorgeReservation.Contracts.Interfaces.Buildings;
using STGeorgeReservation.Contracts.Services;
using STGeorgeReservation.Controllers.CandidatesPortal;
using STGeorgeReservation.DTOs.ResponseDTOs;
using System.Net;

namespace STGeorgeReservation.Controllers.BuildingsController
{
    [Route("api/Buildings")]
    [ApiController]
    public class BuildingsController : UsersBaseApiController
    {
        private readonly IBuildings _buildingService;

        public BuildingsController(IBuildings buildingService)
        {
            _buildingService = buildingService;
        }

        [HttpGet]
        [Route("GetAllBuildings")]
        public async Task<IActionResult> GetAllBuildings()
        {
            var buildings = await _buildingService.GetAllBuildingsAsync();
            return Ok(buildings);
        }

        [HttpGet]
        [Route("GetBuildingById/{id}")]
        public async Task<IActionResult> GetBuildingById(Guid id)
        {
            var building = await _buildingService.GetBuildingByIdAsync(id);

            if (building == null)
                return NotFound("Building not found");

            return Ok(building);
        }


        /// <summary>
        /// Adds or edits job information
        /// </summary>
        /// <param name="model">Job insert model containing job information</param>
        /// <returns>Success or error response</returns>
        [HttpPost("AddBuilding")]
        [ProducesResponseType(typeof(SuccessResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddBuildingAsync([FromBody] AddBuildingRequestsDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = new Error
                    {
                        Message = "Invalid model data"
                    }
                });
            }

            var result = await _buildingService.AddBuildingAsync(model);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return Ok(new SuccessResponse
                {
                    Success = true
                });
            }
            else if (result.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(new ErrorResponse
                {
                    Error = new Error
                    {
                        Message = result.ErrorMessageKey

                    }
                });
            }
            else
            {
                return BadRequest(new ErrorResponse
                {
                    Error = new Error
                    {
                        Message = result.ErrorMessageKey
                    }
                });
            }
        }


    }
}
