using DeviceManager.Application.Features.Devices.Commands.CreateDevice;
using DeviceManager.Application.Features.Devices.Commands.DeleteDevice;
using DeviceManager.Application.Features.Devices.Commands.UpdateDevice;
using DeviceManager.Application.Features.Devices.Queries.GetDeviceById;
using DeviceManager.Application.Features.Devices.Queries.GetDevicesByClientId;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManager.WebApi.Controllers;

[ApiController]
[Route("api/devices")]
public class DeviceController : ControllerBase
{
    private readonly IMediator mediator;

    public DeviceController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetDeviceByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDevice(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            return BadRequest("Device ID cannot be empty.");

        var result = await mediator.Send(new GetDeviceByIdQuery(id), cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Error);
    }

    [HttpGet("client/{id:guid}")]
    [ProducesResponseType(typeof(GetDevicesByClientIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllDevices(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            return BadRequest("Client ID cannot be empty.");

        var result = await mediator.Send(new GetDevicesByClientIdQuery(id), cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Error);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateDeviceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDevice([FromBody] CreateDeviceRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
            return BadRequest("Request body cannot be null.");

        var result = await mediator.Send(request, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(CreateDevice), result.Value, result.Value)
            : BadRequest(result.Error);
    }

    [HttpPut]
    [ProducesResponseType(typeof(UpdateDeviceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateDevice([FromBody] UpdateDeviceRequest request, CancellationToken cancellationToken)
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
    public async Task<IActionResult> DeleteDevice(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            return BadRequest("Device ID cannot be empty.");

        var result = await mediator.Send(new DeleteDeviceRequest(id), cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : NotFound(result.Error);
    }
}
