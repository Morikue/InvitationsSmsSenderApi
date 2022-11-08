using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SmsSenderApi.Exceptions;
using System;
using SmsSenderApi.Enums;

namespace SmsSenderApi.Helpers
{
    public class HttpResultBuilder
    {
        private static Dictionary<ErrorCode?, int> statusCodeByErrorCode = new Dictionary<ErrorCode?, int>
        {
            { ErrorCode.NotInternationalFormatPhoneNumber, 400 },
            { ErrorCode.EmptyPhoneNumbers, 401 },
            { ErrorCode.TooMuchPhoneNumbers, 402 },
            { ErrorCode.TooMuchMessagesToday, 403 },
            { ErrorCode.DuplicatedPhoneNumbers, 404 },
            { ErrorCode.EmptyMessage, 405 },
            { ErrorCode.NotGsmAndTransliteratedCyrillic, 406 },
            { ErrorCode.TooLongMessage, 407 },
            { ErrorCode.IternalErrorStatusCode, 500 },
        };

        private const int okStatusCode = 200;


        private ErrorCode? errorCode;

        private string error;

        private string message;

        public HttpResultBuilder SetErrorCode(ErrorCode errorCode)
        {
            this.errorCode = errorCode;
            return this;
        }

        public HttpResultBuilder SetError(string error)
        {
            this.error = error;
            return this;
        }

        public HttpResultBuilder SetMessage(string message)
        {
            this.message = message;
            return this;
        }

        public ActionResult Build()
        {
            if(this.error != null && this.errorCode != null) // ValidationException
            {
                return new JsonResult(new Dictionary<string, string>()
                        {
                            {"error", this.error },
                            {"message", this.message}
                        })
                { StatusCode = statusCodeByErrorCode[this.errorCode] };
            }
            else if(this.error == null  && this.errorCode != null) // IternalError
            {
                return new JsonResult(this.message) { StatusCode = statusCodeByErrorCode[this.errorCode] };
            }
            else // OkResult
            {
                return new JsonResult(this.message) { StatusCode = okStatusCode };
            }
        }
    }
}
