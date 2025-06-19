using DeviceManager.Application.Features.Events.Commands.CreateEvent;
using DeviceManager.Application.Features.Events.GetEventsByDeviceId;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManager.WebApi.Controllers;

[ApiController]
[Route("api/events")]
public class EventController : ControllerBase
{
    private readonly IMediator mediator;

    public EventController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateEventResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
            return BadRequest("Request body cannot be null.");

        var result = await mediator.Send(request, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(CreateEvent), result.Value, result.Value)
            : BadRequest(result.Error);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetEventsByDeviceIdResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEventsByDeviceId(Guid id, DateTime from, DateTime to, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            return BadRequest("Device ID cannot be empty.");

        var result = await mediator.Send(new GetEventsByDeviceIdQuery(id, from, to), cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.Error);
    }
}
