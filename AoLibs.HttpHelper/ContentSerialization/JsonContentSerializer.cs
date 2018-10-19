using System.Net.Http;
using Newtonsoft.Json;
using AoLibs.HttpHelper.Interfaces;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace AoLibs.HttpHelper.ContentSerialization
{
    public class JsonContentSerializer : IHttpContentSerializer
    {
        DefaultContractResolver _contractResolver;

        public JsonContentSerializer(DefaultContractResolver contractResolver)
        {
            _contractResolver = contractResolver;
        }

        public void AddContent(HttpRequestMessage message, object model)
        {
            var json = JsonConvert.SerializeObject(model, new JsonSerializerSettings() { ContractResolver = _contractResolver });
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
