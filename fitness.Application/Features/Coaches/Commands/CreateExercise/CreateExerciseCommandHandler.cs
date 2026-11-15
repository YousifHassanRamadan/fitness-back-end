using fitness.Application.Interfaces;
using fitness.Domain.Entities;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.CreateExercise;

public class CreateExerciseCommandHandler
    : IRequestHandler<CreateExerciseCommand, int>
{
    private readonly ICoachManagementRepository _repository;

    public CreateExerciseCommandHandler(
        ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(
        CreateExerciseCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return 0;

        var coach = await _repository.GetByUserIdAsync(
            request.UserId,
            cancellationToken);

        if (coach is null)
            return 0;

        var exercise = new Exercise
        {
            CoachId = coach.Id,
            Name = request.Name,
            Description = request.Description,
            MuscleGroup = request.MuscleGroup,
            Equipment = request.Equipment,
            VideoUrl = request.VideoUrl
        };

        await _repository.AddExerciseAsync(
            exercise,
            cancellationToken);

        return exercise.Id;
    }
}