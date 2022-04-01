using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineSoccerManager.Api.DTOs;
using OnlineSoccerManager.Api.Extensions;
using OnlineSoccerManager.Api.ViewModels;
using OnlineSoccerManager.Application.Commands;
using OnlineSoccerManager.Domain.Exceptions;
using OnlineSoccerManager.Domain.Teams;
using System.Security.Claims;

namespace OnlineSoccerManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ApiControllerBase
    {
        ICommand<CreateTeamCommand, Team> _createTeamCommand;
        ICommand<UpdateTeamCommand, Team> _updateTeamCommand;
        private readonly ITeamRepository _teamRepository;

        public TeamsController(ICommand<CreateTeamCommand, Team> command, ICommand<UpdateTeamCommand, Team> updateCommand, ITeamRepository teamRepository)
        {
            _createTeamCommand = command;
            _updateTeamCommand = updateCommand;
            _teamRepository = teamRepository;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<dynamic>> Create([FromBody] CreateTeamDTO model)
        {
            try
            {
                var userId = UserId;

                var team = await _createTeamCommand.ExecuteAsync(new CreateTeamCommand { Country = model.Country, TeamName = model.TeamName, UserId = userId });

                return Ok(team.ToViewModel());
            }
            catch (BusinessException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<dynamic>> Update([FromBody] CreateTeamDTO model, [FromRoute] Guid id)
        {
            try
            {
                var userId = UserId;

                var team = await _updateTeamCommand.ExecuteAsync(new UpdateTeamCommand { TeamId = id, Country = model.Country, Name = model.TeamName, UserId = userId });

                return Ok(team.ToViewModel());
            }
            catch (BusinessException e)
            {
                return StatusCode((int)e.ErrorCode, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("")]
        [Authorize(Roles = "admin,user")]
        public async Task<ActionResult<dynamic>> GetByUserAuthenticated()
        {
            try
            {
                var team = (await _teamRepository.GetAsync(x => x.Owner.Id == UserId)).ToList();

                return Ok(team?.ToViewModel());
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<dynamic>> GetById([FromRoute] Guid id)
        {
            try
            {
                var team = (await _teamRepository.GetByIdAsync(id));

                return Ok(team);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("GetAll")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<dynamic>> GetList()
        {
            try
            {
                var teams = (await _teamRepository?.GetAllAsync())?.ToList();

                return Ok(teams?.ToViewModel());
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
