using fitness.Application.Features.Coaches.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace fitness.Application.Features.Coaches.Queries.GetCoachExerciseById;

public record GetCoachExerciseByIdQuery(
    string UserId,
    int ExerciseId
) : IRequest<ExerciseDto?>;
