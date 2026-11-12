using AutoMapper;
using fitness.Application.Features.Clients.DTOs;
using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Clients.Queries.GetClientProgress;

public class GetClientProgressHandler
    : IRequestHandler<GetClientProgressQuery, List<ProgressRecordDto>>
{
    private readonly IClientManagementRepository _repository;
    private readonly IMapper _mapper;

    public GetClientProgressHandler(
        IClientManagementRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<ProgressRecordDto>> Handle(
        GetClientProgressQuery request,
        CancellationToken cancellationToken)
    {
        var records = await _repository.GetProgressRecordsAsync(
            request.UserId,
            cancellationToken);

        return _mapper.Map<List<ProgressRecordDto>>(records);
    }
}