using Xunit;
using OnlineSoccerManager.Domain.Exceptions;
using OnlineSoccerManager.Domain.Interfaces;
using OnlineSoccerManager.Domain.Players;
using OnlineSoccerManager.Domain.Teams;
using OnlineSoccerManager.Domain.Transfers;
using Moq;
using OnlineSoccerManager.Domain.Builders;
using OnlineSoccerManager.Application.Commands;
using OnlineSoccerManager.Domain.Users;
using System.Threading.Tasks;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineSoccerManager.Application.UnitTest
{
    public class TransferPlayerHandlerTest
    {
        private readonly Mock<ITransferRepository> _transferRepositoryMock;
        private readonly Mock<ITeamRepository> _teamRepositoryMock;
        private readonly Mock<IPlayerRepository> _playerRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        private readonly TransferPlayerCommandHandler _sut;

        public TransferPlayerHandlerTest()
        {
            _transferRepositoryMock = new Mock<ITransferRepository>();
            _teamRepositoryMock = new Mock<ITeamRepository>();
            _playerRepositoryMock = new Mock<IPlayerRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _sut = new TransferPlayerCommandHandler(
                                               _playerRepositoryMock.Object,
                                               _transferRepositoryMock.Object,
                                               _teamRepositoryMock.Object,
                                               _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Execute_WithValidInformation_ShouldTransferedToTheNewTeamAndPriceIncreseadAsync()
        {
            const int sellPrice = 1000;
            const int oldTeamInitialBudget = 0;
            const int newTeamInitialBudget = 1000;

            var oldTeam = new Team { Id = Guid.NewGuid(), Budget = oldTeamInitialBudget };
            var newTeam = new List<Team> { new Team { Id = Guid.NewGuid(), Budget = newTeamInitialBudget, OwnerId = Guid.NewGuid() } };
            var player = new Player { Id = Guid.NewGuid(), TeamId = oldTeam.Id, CurrentValue = 1000 };
            player.SetToTransferList(sellPrice);

            var command = new TransferPlayerCommand()
            {
                PlayerId = oldTeam.Id,
                NewTeamUserId = newTeam[0].OwnerId,
            };

            _playerRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(player);
            _teamRepositoryMock.Setup(m => m.GetByIdAsync(oldTeam.Id)).ReturnsAsync(oldTeam);
            _teamRepositoryMock.Setup(m => m.GetAsync(t => t.Owner.Id == command.NewTeamUserId)).ReturnsAsync(newTeam.AsQueryable());

            var transfer = await _sut.ExecuteAsync(command);

            _playerRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>()));

            _unitOfWorkMock.Verify(m => m.BeginTransactionAsync());

            _transferRepositoryMock.Verify(m => m.AddAsync(transfer));
            _playerRepositoryMock.Verify(m => m.UpdateAsync(player));
            _teamRepositoryMock.Verify(m => m.UpdateAsync(oldTeam));
            _teamRepositoryMock.Verify(m => m.UpdateAsync(newTeam[0]));

            _unitOfWorkMock.Verify(m => m.SaveChangesAsync());

            transfer.Should().NotBeNull();
            transfer.Id.Should().NotBeEmpty();
            transfer.CreatedDate.Should().BeAfter(DateTime.MinValue);
            transfer.SellPrice.Should().BeGreaterThan(0);

            transfer.PlayerId.Should().NotBeEmpty();

            transfer.OldTeamId.Should().NotBeEmpty();
            oldTeam.Budget.Should().Be(oldTeamInitialBudget + sellPrice);

            transfer.NewTeamId.Should().NotBeEmpty();
            newTeam[0].Budget.Should().Be(newTeamInitialBudget - sellPrice);
        }

        [Fact]
        public void Execute_WithInvalidPlayer_ShouldBeReturn_PlayerNotFoundException()
        {
            const int sellPrice = 1000;
            const int oldTeamInitialBudget = 0;
            const int newTeamInitialBudget = 1000;

            var oldTeam = new Team { Id = Guid.NewGuid(), Budget = oldTeamInitialBudget };
            var newTeam = new Team { Id = Guid.NewGuid(), Budget = newTeamInitialBudget };
            Player player = null;

            var command = new TransferPlayerCommand()
            {
                PlayerId = oldTeam.Id,
                NewTeamUserId = newTeam.Id,
            };

            _playerRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(player);

            Assert.ThrowsAsync<PlayerNotFoundException>(async () => await _sut.ExecuteAsync(command));

            _playerRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _playerRepositoryMock.Verify(m => m.UpdateAsync(It.IsAny<Player>()), Times.Never);
            _transferRepositoryMock.Verify(m => m.AddAsync(It.IsAny<Transfer>()), Times.Never);
            _teamRepositoryMock.Verify(m => m.UpdateAsync(oldTeam), Times.Never);
            _teamRepositoryMock.Verify(m => m.UpdateAsync(newTeam), Times.Never);
        }

        [Fact]
        public void Execute_WithInvalidTeam_ShouldBeReturn_TeamNotFoundException()
        {
            const int sellPrice = 1000;
            const int oldTeamInitialBudget = 0;
            const int newTeamInitialBudget = 1000;

            var oldTeam = new Team { Id = Guid.NewGuid(), Budget = oldTeamInitialBudget };
            var newTeam = new Team { Id = Guid.NewGuid(), Budget = newTeamInitialBudget };
            var player = new Player { Id = Guid.NewGuid(), Team = oldTeam, CurrentValue = 1000 };

            var command = new TransferPlayerCommand()
            {
                PlayerId = oldTeam.Id,
                NewTeamUserId = newTeam.Id,
            };

            _playerRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(player);
            //_teamRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(player);

            Assert.ThrowsAsync<TeamNotFoundException>(async () => await _sut.ExecuteAsync(command));

            _playerRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _playerRepositoryMock.Verify(m => m.UpdateAsync(It.IsAny<Player>()), Times.Never);
            _transferRepositoryMock.Verify(m => m.AddAsync(It.IsAny<Transfer>()), Times.Never);
            _teamRepositoryMock.Verify(m => m.UpdateAsync(oldTeam), Times.Never);
            _teamRepositoryMock.Verify(m => m.UpdateAsync(newTeam), Times.Never);
        }

        [Fact]
        public void Execute_InsuffiecientBudget_ShouldBeReturn_NotEnoughtBudgetException()
        {
            const int sellPrice = 1000;
            const int oldTeamInitialBudget = 0;
            const int newTeamInitialBudget = 1;

            var oldTeam = new Team { Id = Guid.NewGuid(), Budget = oldTeamInitialBudget };
            var newTeam = new Team { Id = Guid.NewGuid(), Budget = newTeamInitialBudget };
            var player = new Player { Id = Guid.NewGuid(), Team = oldTeam, CurrentValue = 1000 };
            player.SetToTransferList(sellPrice);

            var command = new TransferPlayerCommand()
            {
                PlayerId = oldTeam.Id,
                NewTeamUserId = newTeam.Id,
            };

            _playerRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(player);
            _teamRepositoryMock.Setup(m => m.GetByIdAsync(oldTeam.Id)).ReturnsAsync(oldTeam);
            _teamRepositoryMock.Setup(m => m.GetByIdAsync(newTeam.Id)).ReturnsAsync(newTeam);

            Assert.ThrowsAsync<NotEnoughtBudgetException>(async () => await _sut.ExecuteAsync(command));

            _playerRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _playerRepositoryMock.Verify(m => m.UpdateAsync(It.IsAny<Player>()), Times.Never);
            _transferRepositoryMock.Verify(m => m.AddAsync(It.IsAny<Transfer>()), Times.Never);
            _teamRepositoryMock.Verify(m => m.UpdateAsync(oldTeam), Times.Never);
            _teamRepositoryMock.Verify(m => m.UpdateAsync(newTeam), Times.Never);
        }

    }
}