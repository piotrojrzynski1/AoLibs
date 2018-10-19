using System;
using System.Net.Http;

namespace AoLibs.HttpHelper.Interfaces
{
    public interface IHttpContentSerializer
    {
        void AddContent(HttpRequestMessage message, object model);
    }
}
