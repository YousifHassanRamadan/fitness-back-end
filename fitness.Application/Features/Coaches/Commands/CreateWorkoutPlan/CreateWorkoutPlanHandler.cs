using fitness.Application.Interfaces;
using fitness.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace fitness.Application.Features.Coaches.Commands.CreateWorkoutPlan;

public class CreateWorkoutPlanHandler
    : IRequestHandler<CreateWorkoutPlanCommand, int>
{
    private readonly ICoachManagementRepository _repository;

    public CreateWorkoutPlanHandler(ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(
        CreateWorkoutPlanCommand request,
        CancellationToken cancellationToken)
    {
        var coach = await _repository.GetByUserIdAsync(
            request.UserId,
            cancellationToken);

        if (coach is null)
            throw new UnauthorizedAccessException("Coach not found.");

        var workoutPlan = new WorkoutPlan
        {
            CoachId = coach.Id,
            Name = request.Name,
            Description = request.Description,
            DurationInWeeks = request.DurationInWeeks
        };

        await _repository.AddWorkoutPlanAsync(
            workoutPlan,
            cancellationToken);

        return workoutPlan.Id;
    }
}
