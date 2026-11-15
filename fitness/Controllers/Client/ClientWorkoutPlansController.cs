using fitness.Application.Features.Clients.Queries.GetClientWorkoutPlans;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace fitness.Controllers.Client;

[ApiController]
[Route("api/clients/me/workout-plans")]
[Authorize]
[Authorize(Roles = "Client")]
public class ClientWorkoutPlansController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientWorkoutPlansController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException("User ID not found.");

    [HttpGet]
    public async Task<IActionResult> GetWorkoutPlans(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetClientWorkoutPlansQuery(CurrentUserId),
            cancellationToken);

        return Ok(result);
    }
}