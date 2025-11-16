using FluentValidation;
using MediatR;

namespace Application.Utils;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task
                .WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();
            
            if (failures.Count != 0)
            {
                var errorMessages = string.Join("; ", failures.Select(f => f.ErrorMessage));
                
                var responseType = typeof(TResponse);
                
                if (responseType == typeof(ApiResponse))
                {
                    return (TResponse)(object)ApiResponse.BadRequest(errorMessages);
                }
                
                if (responseType.IsGenericType && 
                    responseType.GetGenericTypeDefinition() == typeof(ApiResponse<>))
                {
                    var badRequestMethod = responseType.GetMethod("BadRequest", 
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    
                    if (badRequestMethod != null)
                    {
                        return (TResponse)badRequestMethod.Invoke(null, new object[] { errorMessages });
                    }
                }
                
                throw new ValidationException(failures);
            }
        }

        return await next();
    }
}