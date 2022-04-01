using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using OnlineSoccerManager.Api.Exceptions;
using OnlineSoccerManager.Domain.Exceptions;
using OnlineSoccerManager.Infra.CrossCutting.Structs;
using System.Net;
using System.Security.Claims;

namespace OnlineSoccerManager.Api.Controllers
{
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        protected Guid UserId => Guid.Parse(GetClaimValue("UserId"));
        protected string Role => GetClaimValue("Role");


        #region Handlers    
        protected IActionResult HandleCommand<TFailure, TSuccess>(Result<TFailure, TSuccess> result) where TFailure : Exception
                            => result.IsFailure ? HandleFailure(result.Failure) : Ok(result.Success);

        protected IActionResult HandleFailure<T>(T exceptionToHandle) where T : Exception
        {
            if (exceptionToHandle is ValidationException)
                return StatusCode(HttpStatusCode.BadRequest.GetHashCode(), (exceptionToHandle as ValidationException).Errors);

            var exceptionPayload = ExceptionPayload.New(exceptionToHandle);
            return exceptionToHandle is BusinessException ?
                StatusCode(exceptionPayload.ErrorCode.GetHashCode(), exceptionPayload) :
                StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), exceptionPayload);
        }

        protected IActionResult HandleValidationFailure<T>(IList<T> validationFailure) where T : ValidationFailure
            => StatusCode(HttpStatusCode.BadRequest.GetHashCode(), validationFailure);

        #endregion

        #region Utils
        protected string GetCustomHeaderValue(string key)
        {
            StringValues headerValues;

            if (Request.Headers.TryGetValue(key, out headerValues))
            {
                return headerValues.FirstOrDefault();
            }

            return null;
        }

        private string GetClaimValue(string type)
                => ((ClaimsIdentity)HttpContext.User.Identity).FindFirst(type)?.Value;

        //protected IActionResult HandleQuery<TSource, TResult>(Result<Exception, TSource> result)
        //    => result.IsSuccess ? Ok(AutoMapper.Mapper.Map<TSource, TResult>(result.Success)) : HandleFailure(result.Failure);

        #endregion

    }
}
