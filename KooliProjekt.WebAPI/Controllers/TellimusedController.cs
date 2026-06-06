using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Tellimused;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TellimusedController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TellimusedController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/tellimused
        [HttpGet]
        public async Task<List<Tellimus>> List()
        {
            return await _mediator.Send(new List.Query());
        }
    }
}