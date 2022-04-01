using OnlineSoccerManager.Domain.Enums;

namespace OnlineSoccerManager.Api.DTOs
{
    public class UpdatePlayerDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Country Country { get; set; }
    }
}
