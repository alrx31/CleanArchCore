using AutoMapper;
using MediatR;

namespace Core.Handlers;

public abstract class BaseRequestHandler<TRequest, TResponse>(
    IMapper mapper
) : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected IMapper Mapper { get; } = mapper;

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
