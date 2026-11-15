using fitness.Application.Features.Clients.Commands.UpdateClientProfile;
using fitness.Application.Features.Clients.Queries.GetClientCoach;
using fitness.Application.Features.Clients.Queries.GetClientProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace fitness.Controllers.Client;

[ApiController]
[Route("api/clients")]
[Authorize]
[Authorize(Roles = "Client")]
public class ClientProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException("User ID not found.");

    [HttpGet("me")]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetClientProfileQuery(CurrentUserId),
            cancellationToken);

        if (result is null)
            return NotFound(new { message = "Client not found." });

        return Ok(result);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateClientProfileCommand command,
        CancellationToken cancellationToken)
    {
        var updated = await _mediator.Send(
            command with { UserId = CurrentUserId },
            cancellationToken);

        if (!updated)
            return NotFound(new { message = "Client not found." });

        return NoContent();
    }

    [HttpGet("me/coach")]
    public async Task<IActionResult> GetCoach(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetClientCoachQuery(CurrentUserId),
            cancellationToken);

        if (result is null)
            return NotFound(new { message = "No coach assigned." });

        return Ok(result);
    }
}