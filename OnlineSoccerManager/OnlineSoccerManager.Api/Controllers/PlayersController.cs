using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineSoccerManager.Api.DTOs;
using OnlineSoccerManager.Api.ViewModels;
using OnlineSoccerManager.Application.Commands;
using OnlineSoccerManager.Domain.Exceptions;
using OnlineSoccerManager.Domain.Players;

namespace OnlineSoccerManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ApiControllerBase
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ICommand<SetPlayerToTransferListCommand, Player> _setPlayerToTransferListCommand;
        private readonly ICommand<UpdatePlayerCommand, Player> _updatePlayer;
        private readonly ICommand<RemovePlayerFromTransferListCommand, Player> _removePlayerFromTransferListCommand;

        public PlayersController(IPlayerRepository playerRepository,
            ICommand<SetPlayerToTransferListCommand, Player> setPlayerToTransferListCommand,
            ICommand<UpdatePlayerCommand, Player> updatePlayerCommand,
            ICommand<RemovePlayerFromTransferListCommand, Player> removePlayerFromTransferListCommand
            )
        {
            _playerRepository = playerRepository;
            _setPlayerToTransferListCommand = setPlayerToTransferListCommand;
            _updatePlayer = updatePlayerCommand;
            _removePlayerFromTransferListCommand = removePlayerFromTransferListCommand;
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<dynamic>> Update([FromBody] UpdatePlayerDTO model, [FromRoute] Guid id)
        {
            try
            {
                var player = await _updatePlayer.ExecuteAsync(new UpdatePlayerCommand
                {
                    PlayerId = id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Country = model.Country,
                    UserId = UserId
                });

                return Ok(player.ToViewModel());
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

        [HttpPatch]
        [Route("SetOnTransfer")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<dynamic>> SetToTransferList([FromBody] SetToTransferListDTO model)
        {
            try
            {
                var player = await _setPlayerToTransferListCommand.ExecuteAsync(new SetPlayerToTransferListCommand
                {
                    PlayerId = Guid.Parse(model.PlayerId),
                    AskPrice = model.AskPrice
                });

                return Ok(player.ToViewModel());
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

        [HttpPatch]
        [Route("RemoveFromTransfer")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<dynamic>> RemovefromTransferList([FromBody] RemovePlayerDTO model)
        {
            try
            {
                var player = await _removePlayerFromTransferListCommand.ExecuteAsync(new RemovePlayerFromTransferListCommand
                {
                    PlayerId = model.PlayerId,
                });

                return Ok(player.ToViewModel());
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

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<dynamic>> GetById([FromQuery] Guid id)
        {
            try
            {
                var player = (await _playerRepository.GetByIdAsync(id));
                return Ok(player.ToViewModel());
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("Team/{id}")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<dynamic>> GetListByTeamId([FromQuery] string id)
        {
            try
            {
                var players = (await _playerRepository.GetAsync(x => x.Team.Id == Guid.Parse(id))).ToList();
                return Ok(players.ToViewModel());
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
                var players = (await _playerRepository.GetAllAsync()).ToList();
                return Ok(players.ToViewModel());
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("Team/Owner")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<dynamic>> Get()
        {
            try
            {
                var players = (await _playerRepository.GetAsync(x => x.Team.Owner.Id == UserId)).ToList();
                return Ok(players.ToViewModel());
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }
    }
}
