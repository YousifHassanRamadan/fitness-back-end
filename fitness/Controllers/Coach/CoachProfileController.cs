using fitness.Application.Features.Coaches.Commands.UpdateCoachProfile;
using fitness.Application.Features.Coaches.Queries.GetCoachProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace fitness.Controllers.Coach;

[ApiController]
[Route("api/coaches")]
[Authorize]
[Authorize(Roles = "Coach")]
public class CoachProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoachProfileController(IMediator mediator)
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
            new GetCoachProfileQuery(CurrentUserId),
            cancellationToken);

        if (result is null)
            return NotFound(new { message = "Coach not found." });

        return Ok(result);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateCoachProfileCommand command,
        CancellationToken cancellationToken)
    {
        var updated = await _mediator.Send(
            command with { UserId = CurrentUserId },
            cancellationToken);

        if (!updated)
            return NotFound(new { message = "Coach not found." });

        return NoContent();
    }
}