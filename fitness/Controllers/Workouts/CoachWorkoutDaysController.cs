using fitness.Application.Features.Coaches.Commands.AddWorkoutDay;
using fitness.Application.Features.Coaches.WorkoutDays.Commands.UpdateWorkoutDay;
using fitness.Application.Features.Coaches.WorkoutDays.Commands.DeleteWorkoutDay;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace fitness.Controllers.Workouts;

[ApiController]
[Route("api/coaches")]
[Authorize]
[Authorize(Roles = "Coach")]
public class CoachWorkoutDaysController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoachWorkoutDaysController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException("User ID not found.");

    [HttpPost("workout-plans/{workoutPlanId:int}/days")]
    public async Task<IActionResult> AddWorkoutDay(
        int workoutPlanId,
        AddWorkoutDayCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command with
            {
                WorkoutPlanId = workoutPlanId,
                UserId = CurrentUserId
            },
            cancellationToken);

        return Ok(new { id = result });
    }

    [HttpPut("workout-days/{workoutDayId:int}")]
    public async Task<IActionResult> UpdateWorkoutDay(
        int workoutDayId,
        UpdateWorkoutDayCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command with
            {
                UserId = CurrentUserId,
                WorkoutDayId = workoutDayId
            },
            cancellationToken);

        if (!result)
            return NotFound("Workout day not found or invalid data.");

        return NoContent();
    }

    [HttpDelete("workout-days/{workoutDayId:int}")]
    public async Task<IActionResult> DeleteWorkoutDay(
        int workoutDayId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new DeleteWorkoutDayCommand(CurrentUserId, workoutDayId),
            cancellationToken);

        if (!result)
            return NotFound("Workout day not found.");

        return NoContent();
    }
}