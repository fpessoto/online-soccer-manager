using FluentAssertions;
using OnlineSoccerManager.Domain.Players;
using OnlineSoccerManager.Domain.Teams;
using System;
using Xunit;

namespace OnlineSoccerManager.Domain.UnitTest
{
    public class PlayerDomainTest
    {
        [Fact]
        public void Transfer_WithValidInformation_ShouldPlayearBeOnTransferListAndHaveaskPrice()
        {
            Team oldTeam = new() { Id = Guid.NewGuid() };
            Team newTeam = new() { Id = Guid.NewGuid() };
            Player player = new() { Team = oldTeam, CurrentValue = 10 };

            player.Transfer(newTeam);

            player.TeamId.Should().Be(newTeam.Id);
            player.CurrentValue.Should().BeGreaterThan(10);
        }

        [Fact]
        public void SetToTransferList_WithValidInformation_ShouldPlayearBeOnTransferListAndHaveaskPrice()
        {
            int targetAskPrice = 10000;
            var player = new Player();

            player.SetToTransferList(targetAskPrice);

            player.IsOnTransferList.Should().BeTrue();
            player.AskPrice.Should().Be(targetAskPrice);
        }

        [Fact]
        public void SetToTransferList_WithInvalidAskPrice_ShouldThrowArgumentException()
        {
            var player = new Player();

            Assert.Throws<ArgumentException>(() => player.SetToTransferList(0));
            Assert.Throws<ArgumentException>(() => player.SetToTransferList(-1000));
        }

        [Fact]
        public void RemoveFromTransferList_WithValidInformation_ShouldPlayearBeRemovedFromTransferListAndHaveZeroAskPrice()
        {
            var player = new Player();

            player.RemoveFromTransferList();

            player.IsOnTransferList.Should().BeFalse();
            player.AskPrice.Should().Be(0);
        }
    }
}
