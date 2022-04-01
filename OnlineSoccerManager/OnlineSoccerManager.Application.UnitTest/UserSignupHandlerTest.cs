using Xunit;
using FluentAssertions;
using OnlineSoccerManager.Application.Commands;
using System;
using OnlineSoccerManager.Domain.Exceptions;
using System.Threading.Tasks;
using OnlineSoccerManager.Domain.Users;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace OnlineSoccerManager.Application.UnitTest
{
    public class UserSignupHandlerTest
    {
        [Fact]
        public async System.Threading.Tasks.Task Execute_WithValidInformation_ShouldCreateUserAsync()
        {
            var command = new UserSignupCommand
            {
                Email = "newuser@host.com",
                Password = "strongpassword"
            };

            var mock = new Mock<IUserRepository>();

            var sut = new UserSignupCommandHandler(mock.Object);
            var user = await sut.ExecuteAsync(command);

            user.Should().NotBeNull();
            user.Email.Should().Be(command.Email);
            user.Password.Should().Be(command.Password);
            user.Id.Should().NotBe(Guid.Empty);
            user.CreatedDate.Should().BeAfter(DateTime.MinValue);
            //user.Team.Should().BeNull();
        }

        [Fact]
        public async Task Execute_WithExistentEmail_ShouldThrowEmailAlreadyExistsExceptionAsync()
        {
            var command = new UserSignupCommand
            {
                Email = "existent_email@host.com",
                Password = "strongpassword"
            };
            var existentUser = new User();
            existentUser.Create(command.Email, command.Password, "xx", "user");
            var users = new List<User>
            {
                existentUser
            };

            var mock = new Mock<IUserRepository>();
            mock.Setup(x => x.GetAllAsync()).ReturnsAsync(users.AsQueryable());

            var sut = new UserSignupCommandHandler(mock.Object);

            var ex = await Assert.ThrowsAsync<EmailAlreadyExistsException>(async () => await sut.ExecuteAsync(command));
            ex.Message.Should().Be("This e-mail already exists! Try another one.");
        }

        [Fact]
        public void test()
        {
            var a = new int[] { 1,2,3};
            var b = new int[] { 3, 2, 0, 4, 4 };

            var res = FilterBy(a, b);

            res.Should().BeEquivalentTo(new int[] { 2, 3 });

        }

        public static int[] FilterBy(int[] a, int[] b)
        {
            var result = new List<int>();

            return a.Intersect(b).ToArray();
        }
    }
}