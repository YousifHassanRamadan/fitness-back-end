using fitness.Application.Features.Coaches.Commands.CreateMealPlan;
using fitness.Application.Features.Coaches.Commands.UpdateMealPlan;
using fitness.Application.Features.Coaches.Commands.DeleteMealPlan;
using fitness.Application.Features.Coaches.Queries.GetCoachMealPlans;
using fitness.Application.Features.Coaches.Queries.GetCoachMealPlanById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace fitness.Controllers.Meals;

[ApiController]
[Route("api/coaches/meal-plans")]
[Authorize]
[Authorize(Roles = "Coach")]
public class CoachMealPlansController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoachMealPlansController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException("User ID not found.");

    [HttpPost]
    public async Task<IActionResult> CreateMealPlan(
        CreateMealPlanCommand command)
    {
        var mealPlanId = await _mediator.Send(
            command with { UserId = CurrentUserId });

        return Ok(new { id = mealPlanId });
    }

    [HttpGet]
    public async Task<IActionResult> GetMealPlans(
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetCoachMealPlansQuery(CurrentUserId),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{mealPlanId:int}")]
    public async Task<IActionResult> GetMealPlanById(
        int mealPlanId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetCoachMealPlanByIdQuery(CurrentUserId, mealPlanId),
            cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPut("{mealPlanId:int}")]
    public async Task<IActionResult> UpdateMealPlan(
        int mealPlanId,
        UpdateMealPlanCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command with
            {
                UserId = CurrentUserId,
                MealPlanId = mealPlanId
            },
            cancellationToken);

        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{mealPlanId:int}")]
    public async Task<IActionResult> DeleteMealPlan(
        int mealPlanId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new DeleteMealPlanCommand(CurrentUserId, mealPlanId),
            cancellationToken);

        if (!result)
            return NotFound();

        return NoContent();
    }
}