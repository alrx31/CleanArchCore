using AutoMapper;
using Core.Abstractions.Data;
using Core.DI;
using MediatR;

namespace Core.Handlers;

public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected IUnitOfWork UnitOfWork => ServiceLocator.GetService<IUnitOfWork>();
    protected IMapper Mapper => ServiceLocator.GetService<IMapper>();

    
    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}