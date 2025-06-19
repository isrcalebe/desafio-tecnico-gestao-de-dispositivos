using DeviceManager.Application.Features.Clients.Commands;
using DeviceManager.Application.Features.Clients.Queries;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManager.WebApi.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientController : ControllerBase
{
    private readonly IMediator mediator;

    public ClientController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetClientByIdQuery.Response), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetClientById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetClientByIdQuery.Query(id);
        var result = await mediator.Send(query, cancellationToken);

        if (result.IsFailure)
            return NotFound(result.Error.Message);

        return Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientFeature.Request request, CancellationToken cancellationToken)
    {
        if (request is null)
            return BadRequest("Request body cannot be null.");

        var command = new CreateClientFeature.Request(request.Name, request.Email, request.Phone);
        var result = await mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error.Message);

        return CreatedAtAction(nameof(GetClientById), new { id = result.Value }, result.Value);
    }
}
