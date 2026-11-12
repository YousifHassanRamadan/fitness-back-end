using MediatR;

namespace fitness.Application.Features.Coaches.Commands.RemoveClientAssignment;

public record RemoveClientAssignmentCommand(
    string UserId,
    int ClientId
) : IRequest<bool>;