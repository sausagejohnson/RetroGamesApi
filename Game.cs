using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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

        [JsonProperty("platform_ids")]
        public int[] platformIds { get; set; }

        /*public int[] GetPlatformIds
        {
            get
            {
                return this.platformIds;
            }
        }*/

        [JsonProperty("platforms")]
        public List<Platform> Platforms { get; set; }

        public Game()
        {
            Platforms = new List<Platform>();
        }
    }

}
