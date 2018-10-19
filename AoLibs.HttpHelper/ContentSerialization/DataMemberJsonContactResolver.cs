using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AoLibs.HttpHelper.ContentSerialization
{
    public class DataMemberJsonContactResolver : DefaultContractResolver
    {
        public new static readonly DataMemberJsonContactResolver Instance = new DataMemberJsonContactResolver();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            property.ShouldSerialize = p => member.CustomAttributes.Any(q => q.AttributeType == typeof(DataMemberAttribute));
            return property;
        }
    }
}
