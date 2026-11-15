using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.DeleteMeal;

public class DeleteMealCommandHandler
    : IRequestHandler<DeleteMealCommand, bool>
{
    private readonly ICoachManagementRepository _repository;

    public DeleteMealCommandHandler(
        ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        DeleteMealCommand request,
        CancellationToken cancellationToken)
    {
        var meal = await _repository.GetMealByCoachAsync(
            request.MealId,
            request.UserId,
            cancellationToken);

        if (meal == null)
            return false;

        await _repository.DeleteMealAsync(
            meal,
            cancellationToken);

        return true;
    }
}