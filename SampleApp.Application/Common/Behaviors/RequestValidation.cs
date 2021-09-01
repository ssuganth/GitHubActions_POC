using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SampleApp.Application.Internal;

namespace SampleApp.Application.Common.Behaviors
{
    public class RequestValidation<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<RequestValidation<TRequest, TResponse>> _logger;

        public RequestValidation(IEnumerable<IValidator<TRequest>> validators,
            ILogger<RequestValidation<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext(request);
            var requestName = typeof(TRequest).Name;

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.NotNullOrEmpty())
            {
                _logger.LogError("Invalid Request : {Name}", requestName);
                throw new Exception(failures.ToString());
            }

            return next();
        }
    }
}