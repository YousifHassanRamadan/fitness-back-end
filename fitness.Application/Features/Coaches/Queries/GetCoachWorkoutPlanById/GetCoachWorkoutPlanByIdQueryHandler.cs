using fitness.Application.Features.Coaches.DTOs;
using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Coaches.Queries.GetCoachWorkoutPlanById;

public class GetCoachWorkoutPlanByIdQueryHandler
    : IRequestHandler<GetCoachWorkoutPlanByIdQuery, WorkoutPlanDetailsDto?>
{
    private readonly ICoachManagementRepository _repository;

    public GetCoachWorkoutPlanByIdQueryHandler(
        ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorkoutPlanDetailsDto?> Handle(
        GetCoachWorkoutPlanByIdQuery request,
        CancellationToken cancellationToken)
    {
        var plan = await _repository.GetWorkoutPlanDetailsByCoachAsync(
            request.WorkoutPlanId,
            request.UserId,
            cancellationToken);

        if (plan is null)
            return null;

        return new WorkoutPlanDetailsDto
        {
            Id = plan.Id,
            Name = plan.Name,
            Description = plan.Description,
            DurationInWeeks = plan.DurationInWeeks,

            WorkoutDays = plan.WorkoutDays
                .OrderBy(d => d.DayNumber)
                .Select(day => new WorkoutDayDto
                {
                    Id = day.Id,
                    DayNumber = day.DayNumber,
                    Name = day.Name,

                    Exercises = day.WorkoutExercises
                        .OrderBy(e => e.Order)
                        .Select(exercise => new WorkoutExerciseDto
                        {
                            Id = exercise.Id,
                            ExerciseId = exercise.ExerciseId,
                            ExerciseName = exercise.Exercise.Name,
                            Sets = exercise.Sets,
                            Reps = exercise.Reps,
                            RestSeconds = exercise.RestSeconds,
                            Order = exercise.Order
                        }).ToList()
                }).ToList()
        };
    }
}