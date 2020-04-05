using System;
using System.Net;

namespace homebrewAppServerAPI.Domain.ExceptionHandling
{
    public class homebrewAPIException: Exception
    {
        public HttpStatusCode StatusCode { get; }

        public homebrewAPIException(HttpStatusCode statusCode, string errorCode, string errorDescription) : base($"{errorCode}::{errorDescription}")
        {
            StatusCode = statusCode;
        }

        public homebrewAPIException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
