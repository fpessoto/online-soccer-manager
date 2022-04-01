using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineSoccerManager.Api.DTOs;
using OnlineSoccerManager.Api.Services;
using OnlineSoccerManager.Api.ViewModels;
using OnlineSoccerManager.Application.Commands;
using OnlineSoccerManager.Domain.Exceptions;
using OnlineSoccerManager.Domain.Users;

namespace OnlineSoccerManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        ICommand<UserSignupCommand, User> _signupCommand;

        public UserController(IUserRepository userRepository, ICommand<UserSignupCommand, User> command)
        {
            _userRepository = userRepository;
            _signupCommand = command;
        }

        [HttpPost]
        [Route("signup")]
        public async Task<ActionResult<dynamic>> Signup([FromBody] UserAuth model)
        {
            try
            {
                var cmd = new UserSignupCommand
                {
                    Email = model.Email,
                    Password = model.Password,
                    Username = model.Email
                };

                var newUser = await _signupCommand.ExecuteAsync(cmd);
                newUser.Password = "";

                return Ok(newUser);
            }
            catch (BusinessException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] UserAuth model)
        {
            try
            {
                // Recupera o usuário
                var user = await _userRepository.GetAsync(model.Email, model.Password);

                // Verifica se o usuário existe
                if (user == null)
                    return NotFound(new { message = "Usuário ou senha inválidos" });

                // Gera o Token
                var token = TokenService.GenerateToken(user);

                // Oculta a senha
                user.Password = "";

                // Retorna os dados
                return Ok(new
                {
                    user = user.ToViewModel(),
                    token = token
                });
            }
            catch (BusinessException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
