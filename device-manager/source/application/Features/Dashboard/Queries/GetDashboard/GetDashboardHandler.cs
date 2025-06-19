using DeviceManager.Common;
using DeviceManager.Domain;
using DeviceManager.Domain.Repositories;
using Mediator;

namespace DeviceManager.Application.Features.Dashboard.Queries.GetDashboard;

public sealed class GetDashboardHandler : IRequestHandler<GetDashboardQuery, Result<GetDashboardResponse, Error>>
{
    private readonly IEventRepository eventRepository;

    public GetDashboardHandler(IEventRepository eventRepository)
    {
        this.eventRepository = eventRepository;
    }

    public async ValueTask<Result<GetDashboardResponse, Error>> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        var endDateTime = DateTime.UtcNow;
        var startDateTime = endDateTime.AddDays(-7);

        var events = await eventRepository.GetEventsFromLastDaysAsync(7, cancellationToken);

        var eventsList = events.ToList();
        var eventsByType = eventsList
                           .GroupBy(e => e.Type)
                           .ToDictionary(
                               group => group.Key.ToString(),
                               group => group.Count());

        return new GetDashboardResponse(startDateTime, endDateTime, eventsList.Count, eventsByType);
    }
}
