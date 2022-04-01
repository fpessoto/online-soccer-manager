using System.Runtime.Serialization;

namespace OnlineSoccerManager.Domain.Exceptions
{
    public class TeamNotFoundException : BusinessException
    {
        public TeamNotFoundException() : base(Enums.ErrorCodes.NotFound, "Team not found")
        {
        }

        public TeamNotFoundException(string? message) : base(Enums.ErrorCodes.NotFound, message)
        {
        }
    }
}
