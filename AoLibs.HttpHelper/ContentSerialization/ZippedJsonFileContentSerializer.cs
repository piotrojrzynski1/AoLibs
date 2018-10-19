using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using AoLibs.HttpHelper.Interfaces;
using AoLibs.HttpHelper.Statics;

namespace AoLibs.HttpHelper.ContentSerialization
{
    public class ZippedJsonFileContentSerializer : IHttpContentSerializer
    {
        DefaultContractResolver _contractResolver;

        public ZippedJsonFileContentSerializer(DefaultContractResolver contractResolver)
        {
            _contractResolver = contractResolver;
        }

        public void AddContent(HttpRequestMessage message, object model)
        {
            var json = JsonConvert.SerializeObject(model, new JsonSerializerSettings() { ContractResolver = _contractResolver });

            var compressed = ZipUtility.Zip(json);
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(new MemoryStream(compressed)), "data", "data.txt.gz");
            message.Content = content;
        }
    }
}
