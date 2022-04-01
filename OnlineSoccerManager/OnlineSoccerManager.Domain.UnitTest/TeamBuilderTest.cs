using OnlineSoccerManager.Domain.Enums;
using Xunit;
using FluentAssertions;
using System.Linq;
using OnlineSoccerManager.Domain.Builders;
using Moq;
using System;
using OnlineSoccerManager.Domain.Users;
using OnlineSoccerManager.Domain.Players;
using OnlineSoccerManager.Domain.Teams;

namespace OnlineSoccerManager.Domain.UnitTest
{
    public class TeamBuilderTest
    {
        [Fact]
        public void Build_WithValidInformation_ShouldReturnTeam()
        {
            var user = new User { Id = Guid.NewGuid(), Username = "Felipe.Pessoto" };
            string teamName = "Sao Paulo FC";
            var country = Country.Brazil;

            var mockPlayerBuilder = new Mock<PlayerBuilder>();

            var teamBuilder = new TeamBuilder(mockPlayerBuilder.Object);

            Team team = teamBuilder.Build(user, teamName, country);

            team.Should().NotBeNull();
            team.Id.Should().NotBe(Guid.Empty);
            team.CreatedDate.Should().BeAfter(DateTime.MinValue);
            team.UpdatedDate.Should().BeAfter(DateTime.MinValue);
            team.OwnerId.Should().NotBeEmpty();

            team.Name.Should().Be(teamName);
            team.Country.Should().Be(country);
            team.Budget.Should().Be(5000000);

            team.Players.Should().NotBeNull();
            team.Players.Should().HaveCount(20);
            team.Players.Where(p => p.Position == PlayerPosition.Goalkeeper).Should().HaveCount(3);
            team.Players.Where(p => p.Position == PlayerPosition.Defender).Should().HaveCount(6);
            team.Players.Where(p => p.Position == PlayerPosition.Midfielder).Should().HaveCount(6);
            team.Players.Where(p => p.Position == PlayerPosition.Attacker).Should().HaveCount(5);

            team.Value.Should().Be(20000000);
            // team must have:
            //team value, must be the sum of all players value
        }
    }
}