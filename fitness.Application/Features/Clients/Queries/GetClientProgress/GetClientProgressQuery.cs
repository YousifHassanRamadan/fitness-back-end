using fitness.Application.Features.Clients.DTOs;
using MediatR;

namespace fitness.Application.Features.Clients.Queries.GetClientProgress;

public record GetClientProgressQuery(string UserId)
    : IRequest<List<ProgressRecordDto>>;