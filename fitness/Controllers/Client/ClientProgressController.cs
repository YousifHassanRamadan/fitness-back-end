using fitness.Application.Features.Clients.Commands.AddProgressRecord;
using fitness.Application.Features.Clients.Queries.GetClientProgress;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace fitness.Controllers.Client;

[ApiController]
[Route("api/clients/me/progress")]
[Authorize]
[Authorize(Roles = "Client")]
public class ClientProgressController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientProgressController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException("User ID not found.");

    [HttpGet]
    public async Task<IActionResult> GetProgress(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetClientProgressQuery(CurrentUserId),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("me/progress")]
    public async Task<IActionResult> AddProgressRecord(
    [FromBody] AddProgressRecordCommand command,
    CancellationToken cancellationToken)
    {
        var progressRecordId = await _mediator.Send(
            command with { UserId = CurrentUserId },
            cancellationToken);

        if (progressRecordId == 0)
            return NotFound("Client not found.");

        return Ok(new { id = progressRecordId });
    }
}