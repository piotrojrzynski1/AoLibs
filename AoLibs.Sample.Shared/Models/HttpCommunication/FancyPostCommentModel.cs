using System;
using System.Runtime.Serialization;
using AoLibs.HttpHelper.Models;

namespace AoLibs.Sample.Shared.Models.HttpCommunication
{
    [DataContract]
    public class FancyPostCommentModel : ApiResponseModelBase
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long PostId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Body { get; set; }
    }
}
