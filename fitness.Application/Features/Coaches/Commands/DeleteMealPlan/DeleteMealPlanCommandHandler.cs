using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.DeleteMealPlan;

public class DeleteMealPlanCommandHandler
    : IRequestHandler<DeleteMealPlanCommand, bool>
{
    private readonly ICoachManagementRepository _repository;

    public DeleteMealPlanCommandHandler(
        ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        DeleteMealPlanCommand request,
        CancellationToken cancellationToken)
    {
        var mealPlan = await _repository.GetMealPlanDetailsByCoachAsync(
            request.MealPlanId,
            request.UserId,
            cancellationToken);

        if (mealPlan == null)
            return false;

        await _repository.DeleteMealPlanAsync(
            mealPlan,
            cancellationToken);

        return true;
    }
}