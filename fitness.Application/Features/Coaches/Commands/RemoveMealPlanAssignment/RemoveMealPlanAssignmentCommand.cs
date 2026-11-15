using MediatR;

namespace fitness.Application.Features.Coaches.Commands.RemoveMealPlanAssignment;

public record RemoveMealPlanAssignmentCommand(
    string UserId,
    int ClientId,
    int AssignmentId) : IRequest<bool>;