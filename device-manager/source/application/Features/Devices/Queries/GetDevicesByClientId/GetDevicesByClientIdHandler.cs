using DeviceManager.Application.Features.Devices.Queries.GetDeviceById;
using DeviceManager.Common;
using DeviceManager.Domain;
using DeviceManager.Domain.Repositories;
using Mediator;

namespace DeviceManager.Application.Features.Devices.Queries.GetDevicesByClientId;

public sealed class GetDevicesByClientIdHandler : IRequestHandler<GetDevicesByClientIdQuery, Result<GetDevicesByClientIdResponse, Error>>
{
    private readonly IDeviceRepository deviceRepository;

    public GetDevicesByClientIdHandler(IDeviceRepository deviceRepository)
    {
        this.deviceRepository = deviceRepository;
    }

    public async ValueTask<Result<GetDevicesByClientIdResponse, Error>> Handle(GetDevicesByClientIdQuery request, CancellationToken cancellationToken)
    {
        var devices = await deviceRepository.GetByClientIdAsync(request.ClientId, cancellationToken);

        if (!devices.Any())
            return new Error("There is no devices for this client.");

        var devicesResponse = devices.Select(device => new GetDeviceByIdResponse(
            device.Id,
            device.SerialNumber,
            device.IMEI,
            device.ActivatedAt
        ));

        return new GetDevicesByClientIdResponse(devicesResponse);
    }
}
