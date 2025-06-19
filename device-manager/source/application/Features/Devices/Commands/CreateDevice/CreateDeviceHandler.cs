using DeviceManager.Common;
using DeviceManager.Domain;
using DeviceManager.Domain.Entities;
using DeviceManager.Domain.Repositories;
using Mediator;

namespace DeviceManager.Application.Features.Devices.Commands.CreateDevice;

public sealed class CreateDeviceHandler : IRequestHandler<CreateDeviceRequest, Result<CreateDeviceResponse, Error>>
{
    private readonly IClientRepository clientRepository;
    private readonly IDeviceRepository deviceRepository;

    public CreateDeviceHandler(IClientRepository clientRepository, IDeviceRepository deviceRepository)
    {
        this.clientRepository = clientRepository;
        this.deviceRepository = deviceRepository;
    }

    public async ValueTask<Result<CreateDeviceResponse, Error>> Handle(CreateDeviceRequest request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetByIdAsync(request.ClientId, cancellationToken);
        if (client is null)
            return new Error("The specified client does not exist.");

        var allDevices = await deviceRepository.GetAllAsync(cancellationToken);

        if (allDevices.Any(d => d.SerialNumber.Value == request.SerialNumber))
            return new Error("A device with the same serial number already exists.");

        if (allDevices.Any(d => d.IMEI.Value == request.IMEI))
            return new Error("A device with the same IMEI already exists.");

        var result = Device.Create(
            request.SerialNumber,
            request.IMEI,
            request.ClientId
        );

        if (result.IsFailure)
            return result.Error;

        var device = result.Value;
        device.Activate();

        await deviceRepository.AddAsync(device, cancellationToken);

        return new CreateDeviceResponse(device.Id);
    }
}
