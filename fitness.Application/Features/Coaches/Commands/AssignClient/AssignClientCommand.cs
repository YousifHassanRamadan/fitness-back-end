using MediatR;

namespace fitness.Application.Features.Coaches.Commands.AssignClient;

public record AssignClientCommand(
    string UserId,
    int ClientId
) : IRequest<AssignClientResult>;