using System;
using System.Net.Http;

namespace AoLibs.HttpHelper.Interfaces
{
    public interface IHttpMessageHandlerFactory
    {
        HttpMessageHandler GetHandler();
    }
}
