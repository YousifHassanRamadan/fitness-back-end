using fitness.Application.Features.Coaches.Commands.CreateWorkoutPlan;
using fitness.Application.Features.Coaches.Commands.UpdateWorkoutPlan;
using fitness.Application.Features.Coaches.Queries.GetCoachWorkoutPlanById;
using fitness.Application.Features.Coaches.Queries.GetCoachWorkoutPlans;
using fitness.Application.Features.Coaches.WorkoutPlans.Commands.DeleteWorkoutPlan;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace fitness.Controllers.Workouts;

[ApiController]
[Route("api/coaches/workout-plans")]
[Authorize]
[Authorize(Roles = "Coach")]
public class CoachWorkoutPlansController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoachWorkoutPlansController(IMediator mediator)
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
            new GetCoachWorkoutPlansQuery(CurrentUserId),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{workoutPlanId:int}")]
    public async Task<IActionResult> GetWorkoutPlanById(
        int workoutPlanId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetCoachWorkoutPlanByIdQuery(CurrentUserId, workoutPlanId),
            cancellationToken);

        if (result is null)
            return NotFound(new { message = "Workout plan not found." });

        return Ok(result);
    }

    [HttpPut("{workoutPlanId:int}")]
    public async Task<IActionResult> UpdateWorkoutPlan(
        int workoutPlanId,
        [FromBody] UpdateWorkoutPlanCommand command,
        CancellationToken cancellationToken)
    {
        var updated = await _mediator.Send(
            command with
            {
                UserId = CurrentUserId,
                WorkoutPlanId = workoutPlanId
            },
            cancellationToken);

        if (!updated)
            return NotFound(new { message = "Workout plan not found." });

        return NoContent();
    }

    [HttpDelete("{workoutPlanId:int}")]
    public async Task<IActionResult> DeleteWorkoutPlan(
        int workoutPlanId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new DeleteWorkoutPlanCommand(CurrentUserId, workoutPlanId),
            cancellationToken);

        if (!result)
            return NotFound("Workout plan not found.");

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> CreateWorkoutPlan(
    [FromBody] CreateWorkoutPlanCommand command,
    CancellationToken cancellationToken)
    {
        var workoutPlanId = await _mediator.Send(
            command with { UserId = CurrentUserId },
            cancellationToken);

        return Ok(new { id = workoutPlanId });
    }
}