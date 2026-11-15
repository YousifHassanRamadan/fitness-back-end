using AutoMapper;
using fitness.Application.Features.Coaches.DTOs;
using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Coaches.Queries.GetCoachExerciseById;

public class GetCoachExerciseByIdQueryHandler
    : IRequestHandler<GetCoachExerciseByIdQuery, ExerciseDto?>
{
    private readonly ICoachManagementRepository _repository;
    private readonly IMapper _mapper;

    public GetCoachExerciseByIdQueryHandler(
        ICoachManagementRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ExerciseDto?> Handle(
        GetCoachExerciseByIdQuery request,
        CancellationToken cancellationToken)
    {
        var exercise = await _repository.GetExerciseByCoachAsync(
            request.ExerciseId,
            request.UserId,
            cancellationToken);

        if (exercise is null)
            return null;

        return _mapper.Map<ExerciseDto>(exercise);
    }
}