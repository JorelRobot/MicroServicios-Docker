using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Behaviours
{
    public class UnhandledExceptionBehaviuor<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger logger;

        public UnhandledExceptionBehaviuor(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;
                logger.LogError(ex, $"Excepsion no controlada para la ppeticion {requestName}");
                throw;
            }
        }
    }
}
