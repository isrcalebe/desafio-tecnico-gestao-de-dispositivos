using DeviceManager.Common;
using DeviceManager.Domain;
using DeviceManager.Domain.Repositories;
using Mediator;

namespace DeviceManager.Application.Features.Devices.Commands.DeleteDevice;

public sealed class DeleteDeviceHandler : IRequestHandler<DeleteDeviceRequest, Result<DeleteDeviceResponse, Error>>
{
    private readonly IDeviceRepository deviceRepository;

    public DeleteDeviceHandler(IDeviceRepository deviceRepository)
    {
        this.deviceRepository = deviceRepository;
    }

    public async ValueTask<Result<DeleteDeviceResponse, Error>> Handle(DeleteDeviceRequest request, CancellationToken cancellationToken)
    {
        var device = await deviceRepository.GetByIdAsync(request.DeviceId, cancellationToken);
        if (device is null)
            return new Error("Device not found", $"Device with ID {request.DeviceId} does not exist.");

        await deviceRepository.DeleteAsync(device.Id, cancellationToken);

        return new DeleteDeviceResponse(true);
    }
}
