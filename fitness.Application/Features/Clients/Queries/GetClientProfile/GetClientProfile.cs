using fitness.Application.Features.Clients.DTOs;
using MediatR;

namespace fitness.Application.Features.Clients.Queries.GetClientProfile;

public record GetClientProfileQuery(string UserId)
    : IRequest<ClientProfileDto?>;