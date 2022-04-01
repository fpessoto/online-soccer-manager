using OnlineSoccerManager.Domain.Enums;

namespace OnlineSoccerManager.Api.DTOs
{
    public class CreateTeamDTO
    {
        public Country Country { get; set; }
        public string TeamName { get; set; }
    }
}
