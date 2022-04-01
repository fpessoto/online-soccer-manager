using System.Runtime.Serialization;

namespace OnlineSoccerManager.Domain.Exceptions
{
    public class UserAlreadyHasOneTeamException : BusinessException
    {
        public UserAlreadyHasOneTeamException(string? message) : base(Enums.ErrorCodes.AlreadyExists, message)
        {
        }
    }
}
