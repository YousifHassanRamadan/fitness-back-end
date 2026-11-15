using fitness.Application.Interfaces;
using fitness.Application.Features.Coaches.DTOs;
using MediatR;

namespace fitness.Application.Features.Coaches.Queries.GetCoachWorkoutPlans;

public class GetCoachWorkoutPlansQueryHandler
    : IRequestHandler<GetCoachWorkoutPlansQuery, List<WorkoutPlanDto>>
{
    private readonly ICoachManagementRepository _repository;

    public GetCoachWorkoutPlansQueryHandler(
        ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<WorkoutPlanDto>> Handle(
        GetCoachWorkoutPlansQuery request,
        CancellationToken cancellationToken)
    {
        var plans = await _repository.GetWorkoutPlansByCoachAsync(
            request.UserId,
            cancellationToken);

        return plans.Select(plan => new WorkoutPlanDto
        {
            Id = plan.Id,
            Name = plan.Name,
            Description = plan.Description,
            DurationInWeeks = plan.DurationInWeeks
        }).ToList();
    }
}