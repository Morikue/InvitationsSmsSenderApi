using SmsSenderApi.Enums;
using System;

namespace SmsSenderApi.Exceptions
{
    public class ValidationException : Exception
    {
        public ErrorCode ErrorCode { get; }

        public string Error { get; } = null;

        public ValidationException(ErrorCode errorCode, string error, string message) 
            : base(message)
        {
            this.ErrorCode = errorCode;
            this.Error = error;
        }
    }
}
