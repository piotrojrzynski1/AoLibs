using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using AoLibs.HttpHelper.Interfaces;
using Newtonsoft.Json.Serialization;

namespace AoLibs.HttpHelper.ContentSerialization
{
    public class FormContentWithJSONsContentSerializer : IHttpContentSerializer
    {
        DefaultContractResolver _contractResolver;

        public FormContentWithJSONsContentSerializer(DefaultContractResolver contractResolver)
        {
            _contractResolver = contractResolver;
        }

        public void AddContent(HttpRequestMessage message, object model)
        {
            PropertyInfo[] propertyInfos = model.GetType().GetProperties();

            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            foreach (PropertyInfo info in propertyInfos)
            {
                if (info.CustomAttributes.Any(p => p.AttributeType == typeof(DataMemberAttribute)))
                {
                    var propertyValue = info.GetValue(model, null);

                    if (propertyValue != null)
                    {
                        if (info.PropertyType == typeof(string))
                            dictionary.Add(info.Name, propertyValue.ToString());
                        else
                            dictionary.Add(info.Name, JsonConvert.SerializeObject(
                                propertyValue, 
                                new JsonSerializerSettings()
                                {
                                    ContractResolver = _contractResolver
                                }));
                    }
                }
            }

            message.Content = new FormUrlEncodedContent(dictionary);
        }
    }
}
