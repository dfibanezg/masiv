using Newtonsoft.Json;
using System;

namespace Masiv.Entities.Business
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class User
    {
        [JsonProperty("id")]
        internal Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("money")]
        public float Money { get; set; }
    }
}
