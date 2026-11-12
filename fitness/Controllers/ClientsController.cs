using fitness.Application.Features.Clients.Commands.UpdateClientProfile;
using fitness.Application.Features.Clients.Queries.GetClientCoach;
using fitness.Application.Features.Clients.Queries.GetClientMealPlans;
using fitness.Application.Features.Clients.Queries.GetClientProfile;
using fitness.Application.Features.Clients.Queries.GetClientProgress;
using fitness.Application.Features.Clients.Queries.GetClientWorkoutPlans;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace fitness.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientsController(IMediator mediator)
    {
        _mediator = mediator;
    }



    private string CurrentUserId =>
      User.FindFirstValue(ClaimTypes.NameIdentifier)
      ?? throw new UnauthorizedAccessException("User ID not found.");

    [HttpGet("me")]
    public async Task<IActionResult> GetProfile(
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(CurrentUserId))
            return Unauthorized();

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
        if (string.IsNullOrEmpty(CurrentUserId))
            return Unauthorized();

        var updatedCommand = command with
        {
            UserId = CurrentUserId
        };

        var updated = await _mediator.Send(
            updatedCommand,
            cancellationToken);

        if (!updated)
            return NotFound(new { message = "Client not found." });

        return NoContent();
    }

    [HttpGet("me/coach")]
    public async Task<IActionResult> GetCoach(
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(CurrentUserId))
            return Unauthorized();

        var result = await _mediator.Send(
            new GetClientCoachQuery(CurrentUserId),
            cancellationToken);

        if (result is null)
            return NotFound(new { message = "No coach assigned." });

        return Ok(result);
    }

    [HttpGet("me/workout-plans")]
    public async Task<IActionResult> GetWorkoutPlans(
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetClientWorkoutPlansQuery(CurrentUserId),
            cancellationToken);

        return Ok(result);
    }


    [HttpGet("me/meal-plans")]
    public async Task<IActionResult> GetMealPlans(
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetClientMealPlansQuery(CurrentUserId),
            cancellationToken);

        return Ok(result);
    }


    [HttpGet("me/progress")]
    public async Task<IActionResult> GetProgress(
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetClientProgressQuery(CurrentUserId),
            cancellationToken);

        return Ok(result);
    }



}