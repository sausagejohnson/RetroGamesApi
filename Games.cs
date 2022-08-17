using Newtonsoft.Json;
using System.Collections.Generic;

namespace RetroGamesApi
{
    public class Games
    {
        [JsonProperty("games")]
        public IEnumerable<Game> GamesList { get; set; }
    }
}