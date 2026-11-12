using fitness.Application.Interfaces;
using fitness.Domain.Entities;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.AddWorkoutExercise;

public class AddWorkoutExerciseHandler
    : IRequestHandler<AddWorkoutExerciseCommand, int>
{
    private readonly ICoachManagementRepository _repository;

    public AddWorkoutExerciseHandler(ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(
        AddWorkoutExerciseCommand request,
        CancellationToken cancellationToken)
    {
        var workoutDay = await _repository.GetWorkoutDayByCoachAsync(
            request.WorkoutDayId,
            request.UserId,
            cancellationToken);

        if (workoutDay is null)
            throw new KeyNotFoundException("Workout day not found.");

        var exercise = await _repository.GetExerciseByCoachAsync(
            request.ExerciseId,
            request.UserId,
            cancellationToken);

        if (exercise is null)
            throw new KeyNotFoundException("Exercise not found.");

        if (request.Sets <= 0 || request.Reps <= 0 ||
            request.RestSeconds < 0 || request.Order <= 0)
            throw new ArgumentException("Invalid workout exercise values.");

        var workoutExercise = new WorkoutExercise
        {
            WorkoutDayId = workoutDay.Id,
            ExerciseId = exercise.Id,
            Sets = request.Sets,
            Reps = request.Reps,
            RestSeconds = request.RestSeconds,
            Order = request.Order
        };

        await _repository.AddWorkoutExerciseAsync(
            workoutExercise,
            cancellationToken);

        return workoutExercise.Id;
    }
}