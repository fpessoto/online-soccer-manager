using OnlineSoccerManager.Domain.Builders;
using OnlineSoccerManager.Domain.Enums;
using OnlineSoccerManager.Domain.Exceptions;
using OnlineSoccerManager.Domain.Interfaces;
using OnlineSoccerManager.Domain.Players;
using OnlineSoccerManager.Domain.Teams;
using OnlineSoccerManager.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSoccerManager.Application.Commands
{
    public class CreateTeamCommand
    {
        public string TeamName { get; set; }
        public Guid UserId { get; set; }
        public Country Country { get; set; }
    }

    public class CreateTeamCommandHandler : ICommand<CreateTeamCommand, Team>
    {
        private ITeamBuilder _teamBuilder;
        private IUserRepository _userRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTeamCommandHandler(ITeamBuilder teamBuilder,
                                        IUserRepository userRepository,
                                        ITeamRepository teamRepository,
                                        IPlayerRepository playerRepository,
                                        IUnitOfWork unitOfWork)
        {
            _teamBuilder = teamBuilder;
            _userRepository = userRepository;
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Team> ExecuteAsync(CreateTeamCommand command)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId);
            var teams = (await _teamRepository.GetAsync(x => x.Owner.Id == user.Id));

            if (user == null) throw new UserNotFoundException("User not found or does not exist.");
            if (teams != null && teams.Any()) throw new UserAlreadyHasOneTeamException("This user already has an existent team.");

            var team = _teamBuilder.Build(user, command.TeamName, command.Country);

            //save to repository

            await _unitOfWork.BeginTransactionAsync();
            await _teamRepository.AddAsync(team);
            await _playerRepository.AddBatchAsync(team.Players);

            await _unitOfWork.SaveChangesAsync();

            return team;
        }
    }
}
