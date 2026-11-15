using fitness.Application.Features.Clients.Queries.GetClientMealPlans;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace fitness.Controllers.Client;

[ApiController]
[Route("api/clients/me/meal-plans")]
[Authorize]
[Authorize(Roles = "Client")]
public class ClientMealPlansController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientMealPlansController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException("User ID not found.");

    [HttpGet]
    public async Task<IActionResult> GetMealPlans(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetClientMealPlansQuery(CurrentUserId),
            cancellationToken);

        return Ok(result);
    }
}