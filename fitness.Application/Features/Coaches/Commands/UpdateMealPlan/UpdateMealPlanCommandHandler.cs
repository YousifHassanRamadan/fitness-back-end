using AutoMapper;
using fitness.Application.Interfaces;
using fitness.Domain.Entities;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.UpdateMealPlan;

public class UpdateMealPlanCommandHandler
    : IRequestHandler<UpdateMealPlanCommand, bool>
{
    private readonly ICoachManagementRepository _repository;
    private readonly IMapper _mapper;

    public UpdateMealPlanCommandHandler(
        ICoachManagementRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(
        UpdateMealPlanCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Meal plan name is required.");

        if (request.DurationInWeeks <= 0 ||
            request.DurationInDays < 0 ||
            request.DailyCalorieTarget <= 0 ||
            request.ProteinTarget < 0 ||
            request.CarbsTarget < 0 ||
            request.FatTarget < 0)
        {
            throw new ArgumentException("Invalid meal plan values.");
        }

        var mealPlan = await _repository.GetMealPlanDetailsByCoachAsync(
            request.MealPlanId,
            request.UserId,
            cancellationToken);

        if (mealPlan == null)
            return false;

        _mapper.Map(request, mealPlan);

        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}