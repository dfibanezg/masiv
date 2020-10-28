using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Masiv.Entities.Business
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Roulette
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isOpen")]
        public bool IsOpen { get; set; }

        [JsonProperty("bets")]
        public ICollection<Bet> Bets { get; set; }

        [JsonProperty("winnerNumber")]
        public sbyte WinnerNumber { get; set; }

        [JsonProperty("isRedWinnerColor")]
        public bool IsRedWinnerColor { get; set; }
    }
}
