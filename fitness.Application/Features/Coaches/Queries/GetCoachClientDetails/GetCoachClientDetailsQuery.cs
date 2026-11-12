
using fitness.Application.Features.Coaches.DTOs;
using MediatR;

namespace fitness.Application.Features.Coaches.Queries.GetCoachClientDetails;

public record GetCoachClientDetailsQuery(
    string UserId,
    int ClientId) : IRequest<CoachClientDto?>;
