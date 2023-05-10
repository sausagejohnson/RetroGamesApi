using Newtonsoft.Json;
using System;
using System.Collections.Generic;

//Main object class used for serialisation/deserialisation.
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

        public Game()
        {
        }
    }

}
