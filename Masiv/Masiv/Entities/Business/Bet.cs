using Newtonsoft.Json;
using System;

namespace Masiv.Entities.Business
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Bet
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("red")]
        public bool Red { get; set; }

        [JsonProperty("number")]
        public sbyte? Number { get; set; }

        [JsonProperty("money")]
        public float Money { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("gain")]
        public float Gain { get; set; }
    }
}
