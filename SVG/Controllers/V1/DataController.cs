using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SVG.API.Application.Commands.Data;
using SVG.API.Application.Queries.Data;
using SVG.API.Models.Response.Data;

namespace SVG.API.Controllers.V1
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("api/v{api-version:apiVersion}/[controller]/[action]")]
    public class DataController : Controller
    {
        private readonly IMediator _mediator;

        public DataController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<ActionResult<FileReadModel>> SaveFileData([FromBody] SaveFileCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (result == false)
                return BadRequest("Data have been saved.");

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<FileReadModel>> GetFileData([FromQuery] FileGetQuery query, CancellationToken cancellationToken = default)
        {
            var res = await _mediator.Send(query, cancellationToken);
            return Ok(res);
        }
    }
}
