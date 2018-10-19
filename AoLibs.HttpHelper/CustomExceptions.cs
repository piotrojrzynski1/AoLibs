using System;
using System.Net;
using System.Net.Http;

namespace AoLibs.HttpHelper
{
    public class ApiCommunicationException : WebException
    {
        public HttpResponseMessage ResponseMessage { get; set; }
        public ApiCommunicationException() : base() { }
        public ApiCommunicationException(string message) : base(message) { }
        public ApiCommunicationException(string message, Exception innerException) : base(message, innerException) { }
        public ApiCommunicationException(string message, HttpResponseMessage responseMessage) : base(message)
        {
            ResponseMessage = responseMessage;
        }
    }

    public class NoInternetConnectionException : WebException
    {
        public NoInternetConnectionException() : base() { }
        public NoInternetConnectionException(string message) : base(message) { }
        public NoInternetConnectionException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class UnauthorizedException : WebException
    {
        public UnauthorizedException() : base() { }
        public UnauthorizedException(string message) : base(message) { }
        public UnauthorizedException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class ApiCommunicatorRequestFailed : Exception
    {
        public ApiCommunicatorRequestFailed() : base() { }
        public ApiCommunicatorRequestFailed(string message) : base(message) { }
        public ApiCommunicatorRequestFailed(string message, Exception innerException) : base(message, innerException) { }
    }
}
