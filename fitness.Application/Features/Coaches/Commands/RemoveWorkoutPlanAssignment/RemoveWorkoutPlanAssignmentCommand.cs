using MediatR;

namespace fitness.Application.Features.Coaches.Commands.RemoveWorkoutPlanAssignment;

public record RemoveWorkoutPlanAssignmentCommand(
    string UserId,
    int ClientId,
    int AssignmentId
) : IRequest<bool>;