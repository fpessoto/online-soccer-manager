using FluentAssertions;
using Moq;
using OnlineSoccerManager.Application.Commands;
using OnlineSoccerManager.Domain.Exceptions;
using OnlineSoccerManager.Domain.Interfaces;
using OnlineSoccerManager.Domain.Players;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OnlineSoccerManager.Application.UnitTest
{
    public class RemovedPlayerOnTransferHandlerTest
    {
        private readonly Mock<IPlayerRepository> _playerRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        private readonly RemovePlayerFromTransferListCommandHandler _sut;

        public RemovedPlayerOnTransferHandlerTest()
        {
            _playerRepositoryMock = new Mock<IPlayerRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _sut = new RemovePlayerFromTransferListCommandHandler(
                                               _playerRepositoryMock.Object,
                                               _unitOfWorkMock.Object);
        }
        [Fact]
        public async Task Execute_WithValidInformation_ShouldPlayearBeOnTransferListAndHaveaskPriceAsync()
        {
            int targetAskPrice = 10000;

            var command = new RemovePlayerFromTransferListCommand
            {
                PlayerId = Guid.NewGuid()
            };

            Player player = new Player
            {
                Id = command.PlayerId,
            };

            _ = _playerRepositoryMock.Setup(m => m.GetByIdAsync(command.PlayerId)).ReturnsAsync(player);

            var playerResult = await _sut.ExecuteAsync(command);

            playerResult.IsOnTransferList.Should().BeFalse();
            playerResult.AskPrice.Should().Be(0);

            //save player on repo
            _playerRepositoryMock.Verify(mock => mock.UpdateAsync(playerResult));
        }

        [Fact]
        public async Task Execute_WithInvalidPlayer_ShouldBeReturnPlayerNotFoundExceptionAsync()
        {
            var command = new RemovePlayerFromTransferListCommand
            {
                PlayerId = Guid.NewGuid(),
            };
            Player inexistentPlayer = null;

            _ = _playerRepositoryMock.Setup(m => m.GetByIdAsync(command.PlayerId)).ReturnsAsync(inexistentPlayer);

            _ = await Assert.ThrowsAsync<PlayerNotFoundException>(async () => await _sut.ExecuteAsync(command));
        }
    }
}