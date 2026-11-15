using fitness.Application.Features.Coaches.Commands.CreateExercise;
using fitness.Application.Features.Coaches.Commands.UpdateExercise;
using fitness.Application.Features.Coaches.Commands.DeleteExercise;
using fitness.Application.Features.Coaches.Queries.GetCoachExercises;
using fitness.Application.Features.Coaches.Queries.GetCoachExerciseById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace fitness.Controllers.Workouts;

[ApiController]
[Route("api/coaches/exercises")]
[Authorize]
[Authorize(Roles = "Coach")]
public class CoachExercisesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoachExercisesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException("User ID not found.");

    [HttpPost]
    public async Task<IActionResult> CreateExercise(
        CreateExerciseCommand command,
        CancellationToken cancellationToken)
    {
        var exerciseId = await _mediator.Send(
            command with { UserId = CurrentUserId },
            cancellationToken);

        if (exerciseId == 0)
            return BadRequest("Could not create exercise. Check the coach account and exercise name.");

        return Ok(new { ExerciseId = exerciseId });
    }

    [HttpGet]
    public async Task<IActionResult> GetMyExercises(CancellationToken cancellationToken)
    {
        var exercises = await _mediator.Send(
            new GetCoachExercisesQuery(CurrentUserId),
            cancellationToken);

        return Ok(exercises);
    }

    [HttpGet("{exerciseId:int}")]
    public async Task<IActionResult> GetExerciseById(
        int exerciseId,
        CancellationToken cancellationToken)
    {
        var exercise = await _mediator.Send(
            new GetCoachExerciseByIdQuery(CurrentUserId, exerciseId),
            cancellationToken);

        if (exercise is null)
            return NotFound("Exercise not found.");

        return Ok(exercise);
    }

    [HttpPut("{exerciseId:int}")]
    public async Task<IActionResult> UpdateExercise(
        int exerciseId,
        UpdateExerciseCommand command)
    {
        var result = await _mediator.Send(
            command with
            {
                ExerciseId = exerciseId,
                UserId = CurrentUserId
            });

        if (!result)
            return NotFound("Exercise not found.");

        return NoContent();
    }

    [HttpDelete("{exerciseId:int}")]
    public async Task<IActionResult> DeleteExercise(int exerciseId)
    {
        var result = await _mediator.Send(
            new DeleteExerciseCommand(CurrentUserId, exerciseId));

        if (!result)
            return NotFound("Exercise not found.");

        return NoContent();
    }
}