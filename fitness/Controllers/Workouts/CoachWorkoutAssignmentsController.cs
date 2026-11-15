using fitness.Application.Features.Coaches.Commands.AssignWorkoutPlan;
using fitness.Application.Features.Coaches.Commands.RemoveWorkoutPlanAssignment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace fitness.Controllers.Workouts;

[ApiController]
[Route("api/coaches/clients/{clientId:int}/workout-plans")]
[Authorize]
[Authorize(Roles = "Coach")]
public class CoachWorkoutAssignmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoachWorkoutAssignmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException("User ID not found.");

    [HttpPost("{workoutPlanId:int}")]
    public async Task<IActionResult> AssignWorkoutPlan(
        int clientId,
        int workoutPlanId,
        AssignWorkoutPlanCommand command)
    {
        var result = await _mediator.Send(
            command with
            {
                UserId = CurrentUserId,
                ClientId = clientId,
                WorkoutPlanId = workoutPlanId
            });

        if (!result)
            return NotFound("Client or workout plan not found.");

        return NoContent();
    }

    [HttpDelete("{assignmentId:int}")]
    public async Task<IActionResult> RemoveWorkoutPlanAssignment(
        int clientId,
        int assignmentId)
    {
        var result = await _mediator.Send(
            new RemoveWorkoutPlanAssignmentCommand(
                CurrentUserId,
                clientId,
                assignmentId));

        if (!result)
            return NotFound("Workout plan assignment not found.");

        return NoContent();
    }
}