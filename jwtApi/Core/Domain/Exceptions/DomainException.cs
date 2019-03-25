using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace jwtApi.Core.Domain.Exceptions
{
    public enum DomainErrorCode
    {
        UnknownError=1000,
        AuthenticationError,
        PasswordMandatoryError,
        UserAlreadyExistsError,
        KeyNotFoundError,
        RequestValidationError
    }

    // Custom exception class for throwing application specific exceptions (e.g. for validation) 
    // that can be caught and handled within the application
    public class DomainException : Exception
    {
        public readonly DomainErrorCode ErrorCode;

        public DomainException() : base() { }

        public DomainException(string message, DomainErrorCode code = DomainErrorCode.UnknownError) : base(message) {
            ErrorCode = code;
        }

        public DomainException(string message, DomainErrorCode code = DomainErrorCode.UnknownError, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
            ErrorCode = code;
        }
    }

    public class AuthenticationException : DomainException
    {
        public AuthenticationException() : base("Unknown User or Password",DomainErrorCode.AuthenticationError) {}
    }

    public class PasswordMandatoryException : DomainException
    {
        public PasswordMandatoryException() : base("Password is required", DomainErrorCode.PasswordMandatoryError) { }
    }

    public class UserAlreadyExistsException : DomainException
    {
        public UserAlreadyExistsException(string username) : base($"UserName [{username}] already exists", DomainErrorCode.UserAlreadyExistsError) { }
    }

    public class NotFoundException : DomainException
    {
        public NotFoundException(string name, object key) : base($"Entity \"{name}\" ({key}) was not found.", DomainErrorCode.KeyNotFoundError) { }
    }

    public class RequestValidationException : DomainException
    {
        public RequestValidationException()
            : base("One or more validation failures have occurred.", DomainErrorCode.RequestValidationError)
        {
            Failures = new Dictionary<string, string[]>();
        }

        public RequestValidationException(List<ValidationFailure> failures)
            : this()
        {
            var propertyNames = failures
                .Select(e => e.PropertyName)
                .Distinct();

            foreach (var propertyName in propertyNames)
            {
                var propertyFailures = failures
                    .Where(e => e.PropertyName == propertyName)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                Failures.Add(propertyName, propertyFailures);
            }
        }

        public IDictionary<string, string[]> Failures { get; }
    }

}
