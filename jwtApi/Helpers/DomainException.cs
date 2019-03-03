using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace jwtApi.Helpers
{
    public enum DomainErrorCode
    {
        UnknownError=1000,
        AuthenticationError,
        PasswordMandatoryError,
        UserAlreadyExistsError
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
}
