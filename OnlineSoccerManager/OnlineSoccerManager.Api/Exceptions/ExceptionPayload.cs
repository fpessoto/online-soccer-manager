using OnlineSoccerManager.Domain.Enums;
using OnlineSoccerManager.Domain.Exceptions;

namespace OnlineSoccerManager.Api.Exceptions
{
    public class ExceptionPayload
    {
        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
        public static ExceptionPayload New<T>(T exception) where T : Exception
        {
            int errorCode;

            if (exception is BusinessException businessException)
                errorCode = businessException.ErrorCode.GetHashCode();
            else
                errorCode = ErrorCodes.Unhandled.GetHashCode();

            return new ExceptionPayload
            {
                ErrorCode = errorCode,
                ErrorMessage = exception.Message,
            };
        }
    }
}
