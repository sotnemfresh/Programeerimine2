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
        public async Task<IActionResult> List([FromQuery] ListKliendidQuery query)
        {
            var response = await _mediator.Send(query);
            return Result(response);
        }
    }
}