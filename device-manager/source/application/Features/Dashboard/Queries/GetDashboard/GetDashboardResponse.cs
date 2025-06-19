namespace DeviceManager.Application.Features.Dashboard.Queries.GetDashboard;

public record GetDashboardResponse(
    DateTime PeriodStart,
    DateTime PeriodEnd,
    int TotalEvents,
    IDictionary<string, int> EventsByType
);
