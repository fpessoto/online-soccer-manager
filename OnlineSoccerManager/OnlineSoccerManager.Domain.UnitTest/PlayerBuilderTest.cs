using FluentAssertions;
using OnlineSoccerManager.Domain.Builders;
using OnlineSoccerManager.Domain.Players;
using OnlineSoccerManager.Domain.Teams;
using Xunit;

namespace OnlineSoccerManager.Domain.UnitTest
{
    public class PlayerBuilderTest
    {
        [Fact]
        public void Build_WithValidInformation_ShouldReturnNewPlayer()
        {
            var sut = new PlayerBuilder();

            var team = new Team();

            var player = sut.Build(PlayerPosition.Attacker, team);

            player.Should().NotBeNull();
            player.Team.Should().Be(team);
            player.Age.Should().BeInRange(18, 40);
            player.Position.Should().Be(PlayerPosition.Attacker);
            player.FirstName.Should().BeOneOf(sut.FIRST_NAMES);
            player.LastName.Should().BeOneOf(sut.LAST_NAMES);
        }
    }
}
