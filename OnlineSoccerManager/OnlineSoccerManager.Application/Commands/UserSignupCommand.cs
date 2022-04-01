using OnlineSoccerManager.Domain.Exceptions;
using OnlineSoccerManager.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSoccerManager.Application.Commands
{
    public class UserSignupCommand
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }

    public class UserSignupCommandHandler : ICommand<UserSignupCommand, User>
    {
        IUserRepository _userRepository;

        public UserSignupCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> ExecuteAsync(UserSignupCommand command)
        {
            var existentUser = (await _userRepository.GetAllAsync()).Where(x => x.Email == command.Email);

            if (existentUser?.Any() == true) throw new EmailAlreadyExistsException("This e-mail already exists! Try another one.");

            var user = new User();
            user.Create(command.Email, command.Password, command.Username, "user");

            await _userRepository.AddAsync(user);

            return user;
        }
    }
}