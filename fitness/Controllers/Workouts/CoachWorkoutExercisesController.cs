using fitness.Application.Features.Coaches.Commands.AddWorkoutExercise;
using fitness.Application.Features.Coaches.Commands.UpdateWorkoutExercise;
using fitness.Application.Features.Coaches.Commands.RemoveWorkoutExercise;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace fitness.Controllers.Workouts;

[ApiController]
[Route("api/coaches")]
[Authorize]
[Authorize(Roles = "Coach")]
public class CoachWorkoutExercisesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoachWorkoutExercisesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException("User ID not found.");

    [HttpPost("workout-days/{workoutDayId:int}/exercises")]
    public async Task<IActionResult> AddWorkoutExercise(
        int workoutDayId,
        AddWorkoutExerciseCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command with
            {
                WorkoutDayId = workoutDayId,
                UserId = CurrentUserId
            },
            cancellationToken);

        return Ok(new { id = result });
    }

    [HttpPut("workout-exercises/{workoutExerciseId:int}")]
    public async Task<IActionResult> UpdateWorkoutExercise(
        int workoutExerciseId,
        UpdateWorkoutExerciseCommand command)
    {
        var result = await _mediator.Send(
            command with
            {
                WorkoutExerciseId = workoutExerciseId,
                UserId = CurrentUserId
            });

        if (!result)
            return NotFound("Workout exercise not found.");

        return NoContent();
    }

    [HttpDelete("workout-exercises/{workoutExerciseId:int}")]
    public async Task<IActionResult> RemoveWorkoutExercise(
        int workoutExerciseId)
    {
        var result = await _mediator.Send(
            new RemoveWorkoutExerciseCommand(
                CurrentUserId,
                workoutExerciseId));

        if (!result)
            return NotFound("Workout exercise not found.");

        return NoContent();
    }
}