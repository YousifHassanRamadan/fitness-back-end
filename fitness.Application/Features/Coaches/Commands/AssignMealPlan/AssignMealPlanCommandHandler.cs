using fitness.Application.Interfaces;
using fitness.Domain.Entities;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.AssignMealPlan;

public class AssignMealPlanCommandHandler
    : IRequestHandler<AssignMealPlanCommand, bool>
{
    private readonly ICoachManagementRepository _repository;

    public AssignMealPlanCommandHandler(
        ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        AssignMealPlanCommand request,
        CancellationToken cancellationToken)
    {
        var client = await _repository.GetClientDetailsAsync(
            request.UserId,
            request.ClientId,
            cancellationToken);

        if (client == null)
            return false;

        var mealPlan = await _repository.GetMealPlanDetailsByCoachAsync(
            request.MealPlanId,
            request.UserId,
            cancellationToken);

        if (mealPlan == null)
            return false;

        var assignment = new MealPlanAssignment
        {
            ClientId = client.Id,
            MealPlanId = mealPlan.Id,
            AssignedAt = DateTime.UtcNow,
            StartDate = request.StartDate == default
                ? DateTime.UtcNow
                : request.StartDate,
            EndDate = request.EndDate
        };

        await _repository.AddMealPlanAssignmentAsync(
            assignment,
            cancellationToken);

        return true;
    }
}