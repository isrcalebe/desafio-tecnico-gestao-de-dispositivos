using DeviceManager.Application.Features.Clients.Commands.CreateClient;
using DeviceManager.Application.Features.Clients.Commands.DeleteClient;
using DeviceManager.Application.Features.Clients.Commands.UpdateClient;
using DeviceManager.Application.Features.Clients.Queries.GetAllClients;
using DeviceManager.Application.Features.Clients.Queries.GetClientById;
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
    [ProducesResponseType(typeof(GetClientByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetClientById(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            return BadRequest("Client ID cannot be empty.");

        var result = await mediator.Send(new GetClientByIdQuery(id), cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Error);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
            return BadRequest("Request body cannot be null.");

        var result = await mediator.Send(request, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(CreateClient), result.Value, result.Value)
            : BadRequest(result.Error);
    }

    [HttpPut]
    [ProducesResponseType(typeof(UpdateClientResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateClient([FromBody] UpdateClientRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
            return BadRequest("Request body cannot be null.");

        var result = await mediator.Send(request, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Error);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteClient(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            return BadRequest("Client ID cannot be empty.");

        var result = await mediator.Send(new DeleteClientRequest(id), cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : NotFound(result.Error);
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetAllClientsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllClients(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllClientsQuery(), cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Error);
    }
}
