using fitness.Application.Features.Coaches.DTOs;
using fitness.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace fitness.Application.Features.Coaches.Queries.GetCoachExercises;

public class GetCoachExercisesQueryHandler
    : IRequestHandler<GetCoachExercisesQuery, List<ExerciseDto>>
{
    private readonly ICoachManagementRepository _repository;

    public GetCoachExercisesQueryHandler(
        ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ExerciseDto>> Handle(
        GetCoachExercisesQuery request,
        CancellationToken cancellationToken)
    {
        var exercises = await _repository.GetExercisesByCoachAsync(
            request.UserId,
            cancellationToken);

        return exercises.Select(e => new ExerciseDto
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            MuscleGroup = e.MuscleGroup,
            Equipment = e.Equipment,
            VideoUrl = e.VideoUrl
        }).ToList();
    }
}
