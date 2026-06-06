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
        public async Task<IActionResult> List([FromQuery] ListArvedQuery query)
        {
            var response = await _mediator.Send(query);

            return Result(response);
        }
    }
}
