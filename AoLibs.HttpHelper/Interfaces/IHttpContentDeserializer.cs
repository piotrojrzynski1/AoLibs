using System;
using System.Net.Http;
using AoLibs.HttpHelper.Models;

namespace AoLibs.HttpHelper.Interfaces
{
    public interface IHttpContentDeserializer
    {
        T Deserialize<T>(string response);
    }
}
