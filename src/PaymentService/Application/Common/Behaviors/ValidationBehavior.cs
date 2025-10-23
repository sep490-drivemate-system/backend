using FluentValidation;
using MediatR;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                if (failures.Any())
                {
                    var errors = failures.Select(f => f.ErrorMessage).ToList();
                    var errorMessage = string.Join("; ", errors);

                    // Create a Result.Failure response
                    if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
                    {
                        var resultType = typeof(TResponse).GetGenericArguments()[0];
                        var failureMethod = typeof(Result<>).MakeGenericType(resultType)
                            .GetMethod("Failure", new[] { typeof(ServiceError) });

                        var serviceError = ServiceError.BadRequestError(errorMessage);
                        var result = failureMethod?.Invoke(null, new object[] { serviceError });
                        return (TResponse)result!;
                    }
                }
            }

            return await next();
        }
    }
}
