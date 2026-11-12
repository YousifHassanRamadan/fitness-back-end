using fitness.Application.Features.Coaches.Commands.AddWorkoutDay;
using fitness.Application.Features.Coaches.Commands.AddWorkoutExercise;
using fitness.Application.Features.Coaches.Commands.AssignClient;
using fitness.Application.Features.Coaches.Commands.RemoveClientAssignment;
using fitness.Application.Features.Coaches.Commands.UpdateCoachProfile;
using fitness.Application.Features.Coaches.Queries.GetCoachClientDetails;
using fitness.Application.Features.Coaches.Queries.GetCoachClients;
using fitness.Application.Features.Coaches.Queries.GetCoachProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace fitness.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CoachesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoachesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private string CurrentUserId =>
    User.FindFirstValue(ClaimTypes.NameIdentifier)
    ?? throw new UnauthorizedAccessException("User ID not found.");

    // GET: api/coaches/me
    [HttpGet("me")]
    public async Task<IActionResult> GetProfile(
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(CurrentUserId))
            return Unauthorized();

        var result = await _mediator.Send(
            new GetCoachProfileQuery(CurrentUserId),
            cancellationToken);

        if (result is null)
            return NotFound(new { message = "Coach not found." });

        return Ok(result);
    }

    // PUT: api/coaches/me
    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateCoachProfileCommand command,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(CurrentUserId))
            return Unauthorized();

        var updatedCommand = command with
        {
            UserId = CurrentUserId
        };

        var updated = await _mediator.Send(
            updatedCommand,
            cancellationToken);

        if (!updated)
            return NotFound(new { message = "Coach not found." });

        return NoContent();
    }

    // GET: api/coaches/clients
    [HttpGet("clients")]
    public async Task<IActionResult> GetClients(
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(CurrentUserId))
            return Unauthorized();

        var result = await _mediator.Send(
            new GetCoachClientsQuery(CurrentUserId),
            cancellationToken);

        if (result is null)
            return NotFound(new { message = "Coach not found." });

        return Ok(result);
    }

    // GET: api/coaches/clients/{clientId}
    [HttpGet("clients/{clientId:int}")]
    public async Task<IActionResult> GetClientDetails(
        int clientId,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(CurrentUserId))
            return Unauthorized();

        var result = await _mediator.Send(
            new GetCoachClientDetailsQuery(CurrentUserId, clientId),
            cancellationToken);

        if (result is null)
            return NotFound(new { message = "Client not found." });

        return Ok(result);
    }


    [HttpPost("clients/{clientId:int}/assign")]
    public async Task<IActionResult> AssignClient(
    int clientId,
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new AssignClientCommand(CurrentUserId, clientId),
            cancellationToken);

        return result switch
        {
            AssignClientResult.Success =>
                Ok(new { message = "Client assigned successfully." }),

            AssignClientResult.CoachNotFound =>
                NotFound("Coach not found."),

            AssignClientResult.ClientNotFound =>
                NotFound("Client not found."),

            AssignClientResult.AlreadyAssignedToThisCoach =>
                BadRequest("Client is already assigned to you."),

            AssignClientResult.AssignedToAnotherCoach =>
                BadRequest("Client is already assigned to another coach."),

            _ => BadRequest()
        };
    }


    [HttpDelete("clients/{clientId:int}/assign")]
    public async Task<IActionResult> RemoveClientAssignment(
    int clientId,
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new RemoveClientAssignmentCommand(CurrentUserId, clientId),
            cancellationToken);

        if (!result)
            return NotFound("Client not found or not assigned to you.");

        return Ok(new { message = "Client assignment removed successfully." });
    }

    [HttpPost("workout-plans/{workoutPlanId:int}/days")]
    public async Task<IActionResult> AddWorkoutDay(
    int workoutPlanId,
    AddWorkoutDayCommand command,
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command with
            {
                WorkoutPlanId = workoutPlanId,
                UserId = CurrentUserId
            },
            cancellationToken);

        return Ok(new { id = result });
    }


    [HttpPost("workout-days/{workoutDayId:int}/exercises")]
    public async Task<IActionResult> AddWorkoutExercise(
    int workoutDayId,
    AddWorkoutExerciseCommand command,
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command with
            {
                WorkoutDayId = workoutDayId,
                UserId = CurrentUserId
            },
            cancellationToken);

        return Ok(new { id = result });
    }
}