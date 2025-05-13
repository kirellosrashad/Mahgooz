using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STGeorgeReservation.BaseTypes;
using STGeorgeReservation.Contracts.Interfaces.Buildings;
using STGeorgeReservation.Contracts.Interfaces.Reservations;
using STGeorgeReservation.Contracts.Interfaces.Users;
using STGeorgeReservation.Contracts.Services;
using STGeorgeReservation.Controllers.CandidatesPortal;
using STGeorgeReservation.DTOs.ResponseDTOs;
using STGeorgeReservation.Models;
using System.Net;

namespace STGeorgeReservation.Controllers.UsersController
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController : UsersBaseApiController
    {
        private readonly IAuth _UsersService;

        public UsersController(IAuth UserService)
        {
            _UsersService = UserService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _UsersService.RegisterAsync(model);

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
                    Error = new ErrorWithCode
                    {
                        Message = result.ErrorMessageKey,
                        Code = (int)result.StatusCode
                    }
                });
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(DataResponse<AuthModel>), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _UsersService.GetTokenAsync(model);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return Ok(new DataResponse<AuthModel>
                {
                    Data = result.Data,
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
