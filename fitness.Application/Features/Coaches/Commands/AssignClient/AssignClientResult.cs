namespace fitness.Application.Features.Coaches.Commands.AssignClient;

public enum AssignClientResult
{
    Success,
    CoachNotFound,
    ClientNotFound,
    AlreadyAssignedToThisCoach,
    AssignedToAnotherCoach
}