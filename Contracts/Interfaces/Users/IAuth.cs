using STGeorgeReservation.BaseTypes;
using STGeorgeReservation.Models;

namespace STGeorgeReservation.Contracts.Interfaces.Users
{
    public interface IAuth
    {

        Task<OperationResult<AuthModel>> RegisterAsync(RegisterModel model);
        Task<OperationResult<AuthModel>> GetTokenAsync(TokenRequestModel model);
    }
}
