using OnlineSoccerManager.Domain.Enums;
using OnlineSoccerManager.Domain.Exceptions;
using OnlineSoccerManager.Domain.Interfaces;
using OnlineSoccerManager.Domain.Players;
using OnlineSoccerManager.Domain.Teams;

namespace OnlineSoccerManager.Application.Commands
{
    public class UpdateTeamCommand
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
        public Guid TeamId { get; set; }
    }

    public class UpdateTeamCommandHandler : ICommand<UpdateTeamCommand, Team>
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTeamCommandHandler(
                                                     ITeamRepository teamRepository,
                                                     IUnitOfWork unitOfWork)
        {
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Team> ExecuteAsync(UpdateTeamCommand command)
        {
            var team = await _teamRepository.GetByIdAsync(command.TeamId);
            if (team == null) throw new TeamNotFoundException("Team not found or does not exists");
            if (team.OwnerId != command.UserId) throw new BusinessException(ErrorCodes.Unauthorized, "You are not the owner of this team");

            team.Name = command.Name;
            team.Country = command.Country;

            await _unitOfWork.BeginTransactionAsync();
            await _teamRepository.UpdateAsync(team);
            await _unitOfWork.SaveChangesAsync();

            return team;
        }
    }
}
