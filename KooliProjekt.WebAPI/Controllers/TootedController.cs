using KooliProjekt.Application.Features.Tooted;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    public class TootedController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public TootedController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] ListTootedQuery query)
        {
            var response = await _mediator.Send(query);
            return Result(response);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _mediator.Send(new GetToodeQuery { Id = id });
            return Result(response);
        }
        
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveToodeCommand command)
        {
            var response = await _mediator.Send(command);
            return Result(response);
        }
    }
}