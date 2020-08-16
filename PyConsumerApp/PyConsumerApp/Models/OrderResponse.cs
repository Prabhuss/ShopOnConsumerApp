using Newtonsoft.Json;
using System.Runtime.Serialization;
using Xamarin.Forms.Internals;

namespace PyConsumerApp.Models
{
    [Preserve(AllMembers = true)]
    [DataContract]
    public class OrderResponse
    {
        [JsonProperty("status")]
        [DataMember(Name = "status")]

        public string Status { get; set; }

        [JsonProperty("data")]
        [DataMember(Name = "data")]

        public Data Data { get; set; }
    }
    public partial class Data
    {
        [JsonProperty("status")]
        [DataMember(Name = "status")]

        public string Status { get; set; }

        [JsonProperty("message")]
        [DataMember(Name = "message")]

        public string Message { get; set; }
    }
}
