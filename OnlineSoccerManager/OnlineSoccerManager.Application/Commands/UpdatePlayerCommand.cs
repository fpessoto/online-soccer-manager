using OnlineSoccerManager.Domain.Enums;
using OnlineSoccerManager.Domain.Exceptions;
using OnlineSoccerManager.Domain.Interfaces;
using OnlineSoccerManager.Domain.Players;
using OnlineSoccerManager.Domain.Teams;

namespace OnlineSoccerManager.Application.Commands
{
    public class UpdatePlayerCommand
    {
        public Guid PlayerId { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Country Country { get; set; }
    }

    public class UpdatePlayerCommandHandler : ICommand<UpdatePlayerCommand, Player>
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePlayerCommandHandler(IPlayerRepository playerRepository,
                                                     ITeamRepository teamRepository,
                                                     IUnitOfWork unitOfWork)
        {
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Player> ExecuteAsync(UpdatePlayerCommand command)
        {
            var player = await _playerRepository.GetByIdAsync(command.PlayerId);
            if (player == null) throw new PlayerNotFoundException("Player not found or does not exists");

            var team = (await _teamRepository.GetAsync(t => t.OwnerId == command.UserId)).ToList();

            if (team == null) throw new TeamNotFoundException();
            if (team.Where(t => t.Id != player.TeamId).Any()) throw new BusinessException(ErrorCodes.Unauthorized, "You are not the owner of this player");

            player.FirstName = command.FirstName;
            player.LastName = command.LastName;
            player.Country = command.Country;

            await _unitOfWork.BeginTransactionAsync();
            await _playerRepository.UpdateAsync(player);
            await _unitOfWork.SaveChangesAsync();

            return player;
        }
    }
}
