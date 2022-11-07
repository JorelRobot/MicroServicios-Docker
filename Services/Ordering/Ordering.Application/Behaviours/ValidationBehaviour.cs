using FluentValidation;
using MediatR;

namespace Ordering.Application.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            if (validators.Any())
            {
                // Nos traemos el contexto actual
                var context = new ValidationContext<TRequest>(request);

                // Ejecutamos todas la validaciones
                var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                // Vamos a ver si hubo problemas en la validacion
                var failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                // Tronamos los errores de manera controlada
                if (failures.Any())
                    throw new ValidationException(failures);
            }

            return await next();
        }
    }
}
