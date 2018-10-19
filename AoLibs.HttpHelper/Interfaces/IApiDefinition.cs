using System;
using System.Collections.Generic;
using System.Net.Http;

namespace AoLibs.HttpHelper.Interfaces
{
    public interface IApiDefinition
    {
        string BaseAddress { get; }
        void AddHeaders(HttpRequestMessage message);
        IHttpContentSerializer GetContentSerializer();
        IApiResponseHandler GetResponseHandler();
        IApiExceptionHandler GetExceptionHandler();
        IHttpContentDeserializer GetContentDeserializer();
    }
}
