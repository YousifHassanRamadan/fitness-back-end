using fitness.Application.Features.Coaches.Commands.AssignMealPlan;
using fitness.Application.Features.Coaches.Commands.RemoveMealPlanAssignment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace fitness.Controllers.Meals;

[ApiController]
[Route("api/coaches")]
[Authorize]
[Authorize(Roles = "Coach")]
public class CoachMealAssignmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoachMealAssignmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException("User ID not found.");

    // POST: api/coaches/clients/{clientId}/meal-plans/{mealPlanId}
    [HttpPost("clients/{clientId:int}/meal-plans/{mealPlanId:int}")]
    public async Task<IActionResult> AssignMealPlan(
        int clientId,
        int mealPlanId,
        [FromBody] AssignMealPlanCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command with
            {
                UserId = CurrentUserId,
                ClientId = clientId,
                MealPlanId = mealPlanId
            },
            cancellationToken);

        if (!result)
            return NotFound("Client or meal plan not found.");

        return NoContent();
    }

    // DELETE: api/coaches/clients/{clientId}/meal-plans/{assignmentId}
    [HttpDelete("clients/{clientId:int}/meal-plans/{assignmentId:int}")]
    public async Task<IActionResult> RemoveMealPlanAssignment(
        int clientId,
        int assignmentId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new RemoveMealPlanAssignmentCommand(
                CurrentUserId,
                clientId,
                assignmentId),
            cancellationToken);

        if (!result)
            return NotFound("Meal plan assignment not found.");

        return NoContent();
    }
}