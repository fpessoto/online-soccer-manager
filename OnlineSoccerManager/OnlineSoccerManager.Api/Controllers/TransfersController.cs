using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineSoccerManager.Api.DTOs;
using OnlineSoccerManager.Api.ViewModels;
using OnlineSoccerManager.Application.Commands;
using OnlineSoccerManager.Domain.Exceptions;
using OnlineSoccerManager.Domain.Transfers;

namespace OnlineSoccerManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransfersController : ApiControllerBase
    {
        private readonly ITransferRepository _playerRepository;
        private readonly ICommand<TransferPlayerCommand, Transfer> _command;

        public TransfersController(ITransferRepository playerRepository,
            ICommand<TransferPlayerCommand, Transfer> setPlayerToTransferListCommand
            )
        {
            _playerRepository = playerRepository;
            _command = setPlayerToTransferListCommand;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<dynamic>> SetToTransferList([FromBody] TransferPlayerDTO model)
        {
            try
            {
                var transfer = await _command.ExecuteAsync(new TransferPlayerCommand
                {
                    PlayerId = model.PlayerId,
                    NewTeamUserId = UserId,
                });

                return Ok(transfer.ToViewModel());
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
