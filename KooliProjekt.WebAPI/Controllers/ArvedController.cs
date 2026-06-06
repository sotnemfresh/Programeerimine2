using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Arved;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArvedController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ArvedController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/arved
        [HttpGet]
        public async Task<List<Arve>> List()
        {
            return await _mediator.Send(new List.Query());
        }
    }
}