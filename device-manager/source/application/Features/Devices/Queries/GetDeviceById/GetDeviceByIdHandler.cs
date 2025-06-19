using DeviceManager.Common;
using DeviceManager.Domain;
using DeviceManager.Domain.Repositories;
using Mediator;

namespace DeviceManager.Application.Features.Devices.Queries.GetDeviceById;

public sealed class GetDeviceByIdHandler : IRequestHandler<GetDeviceByIdQuery, Result<GetDeviceByIdResponse, Error>>
{
    private readonly IDeviceRepository deviceRepository;

    public GetDeviceByIdHandler(IDeviceRepository deviceRepository)
    {
        this.deviceRepository = deviceRepository;
    }

    public async ValueTask<Result<GetDeviceByIdResponse, Error>> Handle(GetDeviceByIdQuery request, CancellationToken cancellationToken)
    {
        var device = await deviceRepository.GetByIdAsync(request.Id, cancellationToken);

        if (device is null)
            return new Error("Device not found.", $"Device with ID {request.Id} does not exist.");

        return new GetDeviceByIdResponse(device.ClientId, device.SerialNumber, device.IMEI, device.ActivatedAt);
    }
}
