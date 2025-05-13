using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STGeorgeReservation.Contracts.Interfaces.Buildings;
using STGeorgeReservation.Contracts.Interfaces.Reservations;
using STGeorgeReservation.Contracts.Services;
using STGeorgeReservation.Controllers.CandidatesPortal;
using STGeorgeReservation.DTOs.ResponseDTOs;

namespace STGeorgeReservation.Controllers.ReservationController
{
    [Route("api/Reservations")]
    [ApiController]
    public class ReservationsController : UsersBaseApiController
    {
        private readonly IReservations _ReservationService;

        public ReservationsController(IReservations ReservationService)
        {
            _ReservationService = ReservationService;
        }

        [HttpGet]
        [Route("CheckRoomAvailability")]
        public async Task<IActionResult> CheckRoomAvailability(Guid buildingId, Guid floorId, DateTime fromDate, DateTime toDate)
        {
            var availability = await _ReservationService.CheckRoomAvailabilityAsync(buildingId, floorId, fromDate, toDate);
            return Ok(availability);
        }

        [HttpPost]
        [Route("AddReservation")]
        public async Task<IActionResult> AddReservation([FromBody] ReservationsResponseDTO reservationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _ReservationService.AddReservationAsync(reservationDto);
            return Ok("Reservation added successfully");
        }


        [HttpGet("AvailableRooms")]
        public async Task<IActionResult> GetAvailableRooms([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            if (fromDate >= toDate)
            {
                return BadRequest("Invalid date range. The 'fromDate' must be earlier than 'toDate'.");
            }

            var availableRooms = await _ReservationService.GetAvailableRoomsAsync(fromDate, toDate);
            return Ok(availableRooms);
        }
    }
}
