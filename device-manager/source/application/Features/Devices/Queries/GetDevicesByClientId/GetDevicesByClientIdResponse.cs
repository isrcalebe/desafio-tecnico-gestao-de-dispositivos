using DeviceManager.Application.Features.Devices.Queries.GetDeviceById;

namespace DeviceManager.Application.Features.Devices.Queries.GetDevicesByClientId;

public record GetDevicesByClientIdResponse(
    IEnumerable<GetDeviceByIdResponse> Devices);
