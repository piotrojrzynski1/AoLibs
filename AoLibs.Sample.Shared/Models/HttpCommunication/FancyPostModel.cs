using System;
using System.Runtime.Serialization;
using AoLibs.HttpHelper.Models;

namespace AoLibs.Sample.Shared.Models.HttpCommunication
{
    [DataContract]
    public class FancyPostModel : ApiResponseModelBase
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long UserId { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Body { get; set; }
    }
}
