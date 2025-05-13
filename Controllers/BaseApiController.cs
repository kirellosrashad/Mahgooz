using STGeorgeReservation.BaseTypes;
using STGeorgeReservation.Localization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Net;

namespace STGeorgeReservation.Controllers
{
    [ApiController]
 //   [ApiExplorerSettings(GroupName = Constants.SwaggerAllApisGroup)]

    public abstract class BaseApiController : ControllerBase
    {
        protected IMediator _mediator => HttpContext.RequestServices.GetService<IMediator>();

       // protected IStringLocalizer<SharedResource> _localizer => HttpContext.RequestServices.GetService<IStringLocalizer<SharedResource>>();

        //protected string _lang => HttpContext.Request.Headers["Accept-Language"].Count > 0
        //    ? HttpContext.Request.Headers["Accept-Language"][0]
        //    : Constants.EnglishLanguage;

        //protected IActionResult EndpointResult<T>(OperationResult<T> operationResult)
        //{
        //    return operationResult.StatusCode switch
        //    {
        //        HttpStatusCode.OK when operationResult.Paging is not null => Ok(new DataResponseWithPaging<T>
        //        {
        //            Data = operationResult.Data,
        //            Paging = operationResult.Paging
        //        }),
        //        HttpStatusCode.OK => Ok(new DataResponse<T> { Data = operationResult.Data }),
        //        HttpStatusCode.Created => Created(operationResult.RedirectUrl,
        //            new DataResponse<T> { Data = operationResult.Data }),
        //        HttpStatusCode.NotFound => NotFound(new ErrorResponse()
        //        {
        //            Error = new Error()
        //            {
        //                Message = _localizer[operationResult.ErrorMessageKey,
        //                operationResult.ErrorMessagePlaceholderValues ?? Array.Empty<string>()].Value
        //            }
        //        }),
        //        _ => BadRequest(new ErrorResponse()
        //        {
        //            Error = new Error()
        //            {
        //                Message = _localizer[operationResult.ErrorMessageKey,
        //                operationResult.ErrorMessagePlaceholderValues ?? Array.Empty<string>()].Value
        //            }
        //        })
        //    };
        //}

        //protected IActionResult EndpointResult(OperationResult operationResult)
        //{
        //    return operationResult.StatusCode switch
        //    {
        //        HttpStatusCode.OK => Ok(new SuccessResponse { Success = true }),
        //        HttpStatusCode.NotFound => NotFound(new ErrorResponse()
        //        {
        //            Error = new Error
        //            {
        //                Message = _localizer[operationResult.ErrorMessageKey,
        //                operationResult.ErrorMessagePlaceholderValues ?? Array.Empty<string>()].Value
        //            }
        //        }),
        //        _ => BadRequest(new ErrorResponse()
        //        {
        //            Error = new Error
        //            {
        //                Message = _localizer[operationResult.ErrorMessageKey,
        //                operationResult.ErrorMessagePlaceholderValues ?? Array.Empty<string>()].Value
        //            }
        //        })
        //    };
        //}
    }
}
