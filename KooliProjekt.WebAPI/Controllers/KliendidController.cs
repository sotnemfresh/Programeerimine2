using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Kliendid;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KliendidController : ControllerBase
    {
        private readonly IMediator _mediator;

        public KliendidController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/kliendid
        [HttpGet]
        public async Task<List<Klient>> List()
        {
            return await _mediator.Send(new List.Query());
        }
    }
}