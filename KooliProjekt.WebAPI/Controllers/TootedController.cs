using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Tooted;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TootedController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TootedController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/tooted
        [HttpGet]
        public async Task<List<Toode>> List()
        {
            return await _mediator.Send(new List.Query());
        }
    }
}