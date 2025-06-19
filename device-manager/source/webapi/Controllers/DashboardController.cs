using DeviceManager.Application.Features.Dashboard.Queries.GetDashboard;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManager.WebApi.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IMediator mediator;

    public DashboardController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetDashboardResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDashboardData(CancellationToken cancellationToken)
    {
        var query = new GetDashboardQuery();
        var result = await mediator.Send(query, cancellationToken);

        return Ok(result.Value);
    }
}
