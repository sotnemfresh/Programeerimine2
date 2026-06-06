using System.Threading.Tasks;
using KooliProjekt.Application.Features.Arved;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class ArvedController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ArvedController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] ListArvedQuery query)
        {
            var response = await _mediator.Send(query);
            return Result(response);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _mediator.Send(new GetArveQuery { Id = id });
            return Result(response);
        }

    }
}
