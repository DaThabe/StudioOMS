namespace StudioOMS.Messaging;


public interface IRequestHandler<TRequest>
    where TRequest : IRequest
{
    ValueTask HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

public interface IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    ValueTask<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
