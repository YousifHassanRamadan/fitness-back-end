using fitness.Application.Features.Clients.DTOs;
using MediatR;

namespace fitness.Application.Features.Clients.Queries.GetClientCoach;

public record GetClientCoachQuery(string UserId)
    : IRequest<ClientCoachDto?>;