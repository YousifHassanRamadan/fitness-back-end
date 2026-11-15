using fitness.Application.Features.Coaches.Commands.AssignClient;
using fitness.Application.Features.Coaches.Commands.RemoveClientAssignment;
using fitness.Application.Features.Coaches.Queries.GetCoachClientDetails;
using fitness.Application.Features.Coaches.Queries.GetCoachClients;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace fitness.Controllers.Coach;

[ApiController]
[Route("api/coaches/clients")]
[Authorize]
[Authorize(Roles = "Coach")]
public class CoachClientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoachClientsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException("User ID not found.");

    [HttpGet]
    public async Task<IActionResult> GetClients(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetCoachClientsQuery(CurrentUserId),
            cancellationToken);

        if (result is null)
            return NotFound(new { message = "Coach not found." });

        return Ok(result);
    }

    [HttpGet("{clientId:int}")]
    public async Task<IActionResult> GetClientDetails(
        int clientId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetCoachClientDetailsQuery(CurrentUserId, clientId),
            cancellationToken);

        if (result is null)
            return NotFound(new { message = "Client not found." });

        return Ok(result);
    }

    [HttpPost("{clientId:int}/assign")]
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

    [HttpDelete("{clientId:int}/assign")]
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
}