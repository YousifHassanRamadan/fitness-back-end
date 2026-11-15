using fitness.Application.Features.Coaches.Commands.CreateMeal;
using fitness.Application.Features.Coaches.Commands.DeleteMeal;
using fitness.Application.Features.Coaches.Commands.UpdateMeal;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace fitness.Controllers.Meals;

[ApiController]
[Route("api/coaches")]
[Authorize]
[Authorize(Roles = "Coach")]
public class CoachMealsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoachMealsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException("User ID not found.");

    [HttpPost("meal-plans/{mealPlanId:int}/meals")]
    public async Task<IActionResult> CreateMeal(
        int mealPlanId,
        CreateMealCommand command,
        CancellationToken cancellationToken)
    {
        var mealId = await _mediator.Send(
            command with
            {
                UserId = CurrentUserId,
                MealPlanId = mealPlanId
            },
            cancellationToken);

        return Ok(new { id = mealId });
    }

    [HttpPut("meals/{mealId:int}")]
    public async Task<IActionResult> UpdateMeal(
        int mealId,
        UpdateMealCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command with
            {
                UserId = CurrentUserId,
                MealId = mealId
            },
            cancellationToken);

        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("meals/{mealId:int}")]
    public async Task<IActionResult> DeleteMeal(
    int mealId,
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new DeleteMealCommand(CurrentUserId, mealId),
            cancellationToken);

        if (!result)
            return NotFound("Meal not found.");

        return NoContent();
    }
}