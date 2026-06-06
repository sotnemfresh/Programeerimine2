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
        public async Task<IActionResult> List([FromQuery] ListTootedQuery query)
        {
            var response = await _mediator.Send(query);
            return Result(response);
        }
    }
}