using KooliProjekt.Application.Features.Kliendid;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    public class KliendidController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public KliendidController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] ListKliendidQuery query)
        {
            var response = await _mediator.Send(query);
            return Result(response);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _mediator.Send(new GetKlientQuery { Id = id });
            return Result(response);
        }
        
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveKlientCommand command)
        {
            var response = await _mediator.Send(command);
            return Result(response);
        }
    }
}