using System.Runtime.Serialization;

namespace OnlineSoccerManager.Domain.Exceptions
{
    public class PlayerNotFoundException : BusinessException
    {
        public PlayerNotFoundException() : base(Enums.ErrorCodes.AlreadyExists, "Player not found")
        {
        }

        public PlayerNotFoundException(string? message) : base(Enums.ErrorCodes.AlreadyExists, message)
        {
        }

    }
}
