using AutoMapper;
using fitness.Application.Interfaces;
using fitness.Domain.Entities;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.UpdateMeal;

public class UpdateMealCommandHandler
    : IRequestHandler<UpdateMealCommand, bool>
{
    private readonly ICoachManagementRepository _repository;
    private readonly IMapper _mapper;

    public UpdateMealCommandHandler(
        ICoachManagementRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(
        UpdateMealCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Meal name is required.");

        if (request.Calories < 0 ||
            request.Protein < 0 ||
            request.Carbs < 0 ||
            request.Fat < 0)
            throw new ArgumentException("Invalid nutritional values.");

        var meal = await _repository.GetMealByCoachAsync(
            request.MealId,
            request.UserId,
            cancellationToken);

        if (meal == null)
            return false;

        _mapper.Map(request, meal);

        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}