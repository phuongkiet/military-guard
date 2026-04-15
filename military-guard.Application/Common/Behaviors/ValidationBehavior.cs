using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
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

                // Chạy tất cả các Validator của cái Request này
                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                // Gom tất cả các lỗi lại
                var failures = validationResults
                    .Where(r => r.Errors.Any())
                    .SelectMany(r => r.Errors)
                    .ToList();

                if (failures.Any())
                {
                    throw new ValidationException(failures);
                }
            }

            // Nếu dữ liệu ngon lành, cho phép đi tiếp vào Handler
            return await next();
        }
    }
}
