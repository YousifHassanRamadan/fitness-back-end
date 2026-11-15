using fitness.Application.Features.Coaches.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace fitness.Application.Features.Coaches.Queries.GetCoachExercises;

public record GetCoachExercisesQuery(string UserId)
 : IRequest<List<ExerciseDto>>;
