using fitness.Application.Features.Coaches.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace fitness.Application.Features.Coaches.Queries.GetCoachProfile;

public record GetCoachProfileQuery(string UserId)
    : IRequest<CoachProfileDto?>;
