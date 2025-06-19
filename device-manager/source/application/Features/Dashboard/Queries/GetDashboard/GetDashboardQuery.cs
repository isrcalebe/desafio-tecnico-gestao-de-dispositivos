using DeviceManager.Common;
using DeviceManager.Domain;
using Mediator;

namespace DeviceManager.Application.Features.Dashboard.Queries.GetDashboard;

public record GetDashboardQuery() : IRequest<Result<GetDashboardResponse, Error>>;
