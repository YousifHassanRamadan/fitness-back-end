using fitness.Application.Features.Coaches.Commands.AddMealFood;
using fitness.Application.Features.Coaches.Commands.UpdateMealFood;
using fitness.Application.Features.Coaches.Commands.DeleteMealFood;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace fitness.Controllers.Meals;

[ApiController]
[Route("api/coaches")]
[Authorize]
[Authorize(Roles = "Coach")]
public class CoachMealFoodsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoachMealFoodsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException("User ID not found.");

    // POST: api/coaches/meals/{mealId}/foods
    [HttpPost("meals/{mealId:int}/foods")]
    public async Task<IActionResult> AddMealFood(
        int mealId,
        [FromBody] AddMealFoodCommand command,
        CancellationToken cancellationToken)
    {
        var foodId = await _mediator.Send(
            command with
            {
                UserId = CurrentUserId,
                MealId = mealId
            },
            cancellationToken);

        if (foodId == 0)
            return NotFound("Meal not found.");

        return Ok(new { id = foodId });
    }

    // PUT: api/coaches/meal-foods/{mealFoodId}
    [HttpPut("meal-foods/{mealFoodId:int}")]
    public async Task<IActionResult> UpdateMealFood(
        int mealFoodId,
        [FromBody] UpdateMealFoodCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command with
            {
                UserId = CurrentUserId,
                MealFoodId = mealFoodId
            },
            cancellationToken);

        if (!result)
            return NotFound("Meal food not found.");

        return NoContent();
    }

    // DELETE: api/coaches/meal-foods/{mealFoodId}
    [HttpDelete("meal-foods/{mealFoodId:int}")]
    public async Task<IActionResult> DeleteMealFood(
        int mealFoodId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new DeleteMealFoodCommand(
                CurrentUserId,
                mealFoodId),
            cancellationToken);

        if (!result)
            return NotFound("Meal food not found.");

        return NoContent();
    }
}