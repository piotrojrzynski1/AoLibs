using System;
using AoLibs.HttpHelper.Interfaces;
using AoLibs.HttpHelper.Models;
using Newtonsoft.Json;

namespace AoLibs.HttpHelper.ContentSerialization
{
    public class JsonContentDeserializer : IHttpContentDeserializer
    {
        public T Deserialize<T>(string response)
        {
            return JsonConvert.DeserializeObject<T>(response, new JsonSerializerSettings() { });
        }
    }
}
