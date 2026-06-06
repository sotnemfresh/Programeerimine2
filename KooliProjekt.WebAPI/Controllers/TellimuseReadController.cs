using KooliProjekt.Application.Features.TellimuseRead;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    public class TellimuseReadController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public TellimuseReadController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] ListTellimuseReadQuery query)
        {
            var response = await _mediator.Send(query);
            return Result(response);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _mediator.Send(new GetTellimuseReadQuery { Id = id });
            return Result(response);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveTellimuseReadCommand command)
        {
            var response = await _mediator.Send(command);
            return Result(response);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteTellimuseReadCommand command)
        {
            var response = await _mediator.Send(command);
            return Result(response);
        }
    }
}