using Newtonsoft.Json;

namespace Masiv.Entities.DataTransfer
{
    public class BetDto
    {
        [JsonProperty("color")]
        public bool Red { get; set; }

        [JsonProperty("number")]
        public sbyte Number { get; set; }

        [JsonProperty("money")]
        public float Money { get; set; }
    }
}
