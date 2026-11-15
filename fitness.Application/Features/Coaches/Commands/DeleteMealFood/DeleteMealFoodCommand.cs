using MediatR;

namespace fitness.Application.Features.Coaches.Commands.DeleteMealFood;

public record DeleteMealFoodCommand(
    string UserId,
    int MealFoodId) : IRequest<bool>;