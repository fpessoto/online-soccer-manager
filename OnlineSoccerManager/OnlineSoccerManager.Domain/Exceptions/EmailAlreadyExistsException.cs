using OnlineSoccerManager.Domain.Enums;
using System.Runtime.Serialization;

namespace OnlineSoccerManager.Domain.Exceptions
{
    public class EmailAlreadyExistsException : BusinessException
    {
        public EmailAlreadyExistsException() : base(ErrorCodes.AlreadyExists, "")
        {
        }

        public EmailAlreadyExistsException(string? message) : base(ErrorCodes.AlreadyExists, message)
        {
        }

    }
}
