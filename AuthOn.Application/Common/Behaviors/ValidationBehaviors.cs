﻿using ErrorOr;
using FluentValidation;
using MediatR;

namespace AuthOn.Application.Common.Behaviors
{
    public class ValidationBehaviors<TRequest, TResponse>(IValidator<TRequest>? validator = null) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : IErrorOr
    {
        private readonly IValidator<TRequest>? _validator = validator;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validator is null)
            {
                return await next(cancellationToken);
            }

            var validatorResult = await _validator.ValidateAsync(request, cancellationToken);

            if (validatorResult.IsValid)
            {
                return await next(cancellationToken);
            }

            var errors = validatorResult.Errors
                .ConvertAll(validationFailure => Error.Validation(
                    validationFailure.PropertyName,
                    validationFailure.ErrorMessage));

            return (dynamic)errors;
        }
    }
}