
using fitness.Application.Features.Coaches.DTOs;
using MediatR;

namespace fitness.Application.Features.Coaches.Queries.GetCoachClients;

public record GetCoachClientsQuery(string UserId)
    : IRequest<List<CoachClientDto>?>;
