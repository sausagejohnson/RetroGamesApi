using Newtonsoft.Json;
using RetroGamesApi.DTOs;
using System.Collections.Generic;

namespace RetroGamesApi
{
    //Main wrapper object class used for serialisation/deserialisation.
    public class Games
    {
        [JsonProperty("games")]
        public List<Game> GamesList { get; set; }
    }
}