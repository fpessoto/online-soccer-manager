using OnlineSoccerManager.Domain.Exceptions;
using OnlineSoccerManager.Domain.Interfaces;
using OnlineSoccerManager.Domain.Players;
using OnlineSoccerManager.Domain.Teams;
using OnlineSoccerManager.Domain.Transfers;

namespace OnlineSoccerManager.Application.Commands
{
    public class TransferPlayerCommand
    {
        public Guid PlayerId { get; set; }
        public Guid NewTeamUserId { get; set; }
    }

    public class TransferPlayerCommandHandler : ICommand<TransferPlayerCommand, Transfer>
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ITransferRepository _transferRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransferPlayerCommandHandler(
            IPlayerRepository playerRepository,
            ITransferRepository transferRepository,
            ITeamRepository teamRepository,
            IUnitOfWork unitOfWork)
        {
            _playerRepository = playerRepository;
            _transferRepository = transferRepository;
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Transfer> ExecuteAsync(TransferPlayerCommand command)
        {
            var player = await _playerRepository.GetByIdAsync(command.PlayerId);
            if (player == null) throw new PlayerNotFoundException();

            if (!player.IsOnTransferList) throw new PlayerNotInTransferList();

            var newPlayerTeam = (await _teamRepository.GetAsync(t => t.Owner.Id == command.NewTeamUserId)).FirstOrDefault();
            if (newPlayerTeam == null) throw new TeamNotFoundException();

            var oldTeam = (await _teamRepository.GetByIdAsync(player.TeamId));

            if (newPlayerTeam.Budget < player.AskPrice) throw new NotEnoughtBudgetException();

            var transfer = new Transfer
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                PlayerId = player.Id,
                NewTeamId = newPlayerTeam.Id,
                OldTeamId = oldTeam.Id,
                SellPrice = player.AskPrice,
                UpdatedDate = DateTime.Now
            };

            oldTeam.CreditBudget(transfer.SellPrice);
            newPlayerTeam.DebtBudget(transfer.SellPrice);

            player.Transfer(newPlayerTeam);

            await _unitOfWork.BeginTransactionAsync();

            await _playerRepository.UpdateAsync(player);

            await _teamRepository.UpdateAsync(oldTeam);
            await _teamRepository.UpdateAsync(newPlayerTeam);

            await _transferRepository.AddAsync(transfer);

            await _unitOfWork.SaveChangesAsync();

            return transfer;
        }
    }

}
