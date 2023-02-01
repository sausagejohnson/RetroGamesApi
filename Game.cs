using Newtonsoft.Json;
using System;

namespace RetroGamesApi
{
    public class Game
    {
        [JsonProperty("game_id")]
        public int GameId{ get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }
    }

    public class RawGame
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }
    }
}
