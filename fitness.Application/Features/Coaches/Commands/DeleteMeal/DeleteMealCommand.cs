using MediatR;

namespace fitness.Application.Features.Coaches.Commands.DeleteMeal;

public record DeleteMealCommand(
    string UserId,
    int MealId) : IRequest<bool>;