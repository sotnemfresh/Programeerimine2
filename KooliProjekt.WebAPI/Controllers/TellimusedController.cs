using KooliProjekt.Application.Features.Tellimused;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    public class TellimusedController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public TellimusedController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] ListTellimusedQuery query)
        {
            var response = await _mediator.Send(query);
            return Result(response);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _mediator.Send(new GetTellimusQuery { Id = id });
            return Result(response);
        }
        
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveTellimusCommand command)
        {
            var response = await _mediator.Send(command);
            return Result(response);
        }
    }
}