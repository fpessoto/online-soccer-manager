using OnlineSoccerManager.Domain.Exceptions;
using OnlineSoccerManager.Domain.Interfaces;
using OnlineSoccerManager.Domain.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSoccerManager.Application.Commands
{
    public class SetPlayerToTransferListCommand
    {
        public Guid PlayerId { get; set; }
        public decimal AskPrice { get; set; }
    }

    public class SetPlayerToTransferListCommandHandler : ICommand<SetPlayerToTransferListCommand, Player>
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SetPlayerToTransferListCommandHandler(IPlayerRepository playerRepository,
                                                     IUnitOfWork unitOfWork)
        {
            _playerRepository = playerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Player> ExecuteAsync(SetPlayerToTransferListCommand command)
        {
            var player = await _playerRepository.GetByIdAsync(command.PlayerId);
            if (player == null) throw new PlayerNotFoundException("Player not found or does not exists");

            player.SetToTransferList(command.AskPrice);
            
            await _unitOfWork.BeginTransactionAsync();
            await _playerRepository.UpdateAsync(player);
            await _unitOfWork.SaveChangesAsync();

            return player;
        }
    }
}
