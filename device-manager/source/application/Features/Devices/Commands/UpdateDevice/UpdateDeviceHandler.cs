using DeviceManager.Common;
using DeviceManager.Domain;
using DeviceManager.Domain.Repositories;
using DeviceManager.Domain.ValueObjects;
using Mediator;

namespace DeviceManager.Application.Features.Devices.Commands.UpdateDevice;

public sealed class UpdateDeviceHandler : IRequestHandler<UpdateDeviceRequest, Result<UpdateDeviceResponse, Error>>
{
    private readonly IDeviceRepository deviceRepository;

    public UpdateDeviceHandler(IDeviceRepository deviceRepository)
    {
        this.deviceRepository = deviceRepository;
    }

    public async ValueTask<Result<UpdateDeviceResponse, Error>> Handle(UpdateDeviceRequest request, CancellationToken cancellationToken)
    {
        var device = await deviceRepository.GetByIdAsync(request.DeviceId, cancellationToken);
        if (device is null)
            return new Error("Device not found", $"Client with ID {request.DeviceId} does not exist.");

        var allDevices = await deviceRepository.GetAllAsync(cancellationToken);

        if (request.SerialNumber is not null)
        {
            var newSerialNumber = SerialNumber.Create(request.SerialNumber);
            if (newSerialNumber.IsFailure)
                return newSerialNumber.Error;

            if (allDevices.Any(d => d.SerialNumber.Value == newSerialNumber.Value.Value))
                return new Error("A device with the same serial number already exists.");

            device.UpdateSerialNumber(newSerialNumber.Value);
        }

        if (request.Imei is not null)
        {
            var newImei = IMEI.Create(request.Imei);
            if (newImei.IsFailure)
                return newImei.Error;

            if (allDevices.Any(d => d.IMEI.Value == newImei.Value.Value))
                return new Error("A device with the same IMEI already exists.");

            device.UpdateIMEI(newImei.Value);
        }

        await deviceRepository.UpdateAsync(device, cancellationToken);

        return new UpdateDeviceResponse(true);
    }
}
