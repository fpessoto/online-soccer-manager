using Xunit;
using FluentAssertions;
using OnlineSoccerManager.Application.Commands;
using System;
using OnlineSoccerManager.Domain.Enums;
using Moq;
using OnlineSoccerManager.Domain.Builders;
using OnlineSoccerManager.Domain.Users;
using System.Threading.Tasks;
using OnlineSoccerManager.Domain.Exceptions;
using OnlineSoccerManager.Domain.Teams;
using OnlineSoccerManager.Domain.Players;
using OnlineSoccerManager.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace OnlineSoccerManager.Application.UnitTest
{
    public class CreateTeamHandlerTest : IDisposable
    {
        private readonly Mock<ITeamBuilder>? _teamBuilderMock;
        private readonly Mock<IUserRepository>? _userRepositoryMock;
        private readonly Mock<ITeamRepository> _teamRepositoryMock;
        private readonly Mock<IPlayerRepository> _playerRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        private readonly CreateTeamCommandHandler _sut;

        public CreateTeamHandlerTest()
        {
            _teamBuilderMock = new Mock<ITeamBuilder>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _teamRepositoryMock = new Mock<ITeamRepository>();
            _playerRepositoryMock = new Mock<IPlayerRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _sut = new CreateTeamCommandHandler(_teamBuilderMock.Object,
                                               _userRepositoryMock.Object,
                                               _teamRepositoryMock.Object,
                                               _playerRepositoryMock.Object,
                                               _unitOfWorkMock.Object);
        }

        public void Dispose()
        {
        }

        [Fact]
        public async Task Execute_WithValidInformation_ShouldCreateAndReturnaNewTeam()
        {
            var user = new User();
            user.Create("SPFC", "...", "username", "user");

            var command = new CreateTeamCommand
            {
                TeamName = "SPFC",
                UserId = user.Id,
                Country = Country.Brazil,
            };
            Team expectedTeam = CreateDummyTeam(user);

            _userRepositoryMock.Setup(mock => mock.GetByIdAsync(command.UserId)).ReturnsAsync(user);

            _teamBuilderMock.Setup(mock => mock.Build(user, command.TeamName, command.Country)).Returns(
                expectedTeam);

            var team = await _sut.ExecuteAsync(command);

            _teamBuilderMock.Verify(x => x.Build(user, command.TeamName, command.Country), Times.Once);
            _teamRepositoryMock.Verify(mock => mock.AddAsync(team), Times.Once);
            _playerRepositoryMock.Verify(mock => mock.AddBatchAsync(team.Players), Times.Once);

            team.Should().NotBeNull();
            team.Name.Should().Be(command.TeamName);
            team.Id.Should().NotBe(Guid.Empty);
            team.CreatedDate.Should().BeAfter(DateTime.MinValue);
            team.UpdatedDate.Should().BeAfter(DateTime.MinValue);
            team.Owner.Should().NotBeNull();
            team.Owner?.Id.Should().Be(command.UserId);
            team.Budget.Should().BeGreaterThan(0);

            team.Players.Should().NotBeNull();
        }

        private static Team CreateDummyTeam(User user)
        {
            return new Team
            {
                Id = Guid.NewGuid(),
                Country = Country.Brazil,
                Name = "SPFC",
                Budget = 5000000,
                Owner = user,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Players = new List<Player> { CreateDummyPlayer(), CreateDummyPlayer() }
            };
        }

        private static Player CreateDummyPlayer()
        {
            return new Player();
        }

        [Fact]
        public async Task ExecuteAsync_UserWithExistentTeam_ShouldThrowUserAlreadyHasOneTeamException()
        {
            var user = new User();
            user.Create("SPFC", "...", "username", "user");
            var team = new Team();

            var command = new CreateTeamCommand
            {
                TeamName = "SPFC",
                UserId = user.Id,
                Country = Country.Brazil,
            };
            List<Team> teams = new() { team };
            IQueryable<Team> teamsQueryable = teams.AsQueryable();


            var teamBuilderMock = new Mock<ITeamBuilder>();
            var userRepositoryMock = new Mock<IUserRepository>();

            _userRepositoryMock.Setup(mock => mock.GetByIdAsync(command.UserId)).ReturnsAsync(user);
            _teamRepositoryMock.Setup(mock => mock.GetAsync(It.IsAny<Expression<Func<Team, bool>>>())).ReturnsAsync(teamsQueryable);

            var ex = await Assert.ThrowsAsync<UserAlreadyHasOneTeamException>(
                async () => await _sut.ExecuteAsync(command));
            ex.Message.Should().Be("This user already has an existent team.");

            teamBuilderMock.Verify(x => x.Build(user, command.TeamName, command.Country), Times.Never);
        }

        [Fact]
        public async Task Execute_WithInvalidUser_ShouldReturnNotFoundedUserException()
        {
            var command = new CreateTeamCommand
            {
                TeamName = "SPFC",
                UserId = Guid.NewGuid(),
                Country = Country.Brazil,
            };

            var teamBuilderMock = new Mock<ITeamBuilder>();
            var userRepositoryMock = new Mock<IUserRepository>();

            User user = null;

            _userRepositoryMock.Setup(mock => mock.GetByIdAsync(command.UserId)).ReturnsAsync(user);

            var ex = await Assert.ThrowsAsync<UserNotFoundException>(
                async () => await _sut.ExecuteAsync(command));
            ex.Message.Should().Be("User not found or does not exist.");

            teamBuilderMock.Verify(x => x.Build(new User(), command.TeamName, command.Country), Times.Never);
        }
    }
}