using Newtonsoft.Json;
using System.Collections.Generic;

namespace RetroGamesApi
{
    public class Games
    {
        [JsonProperty("games")]
        public List<Game> GamesList { get; set; }
    }
}