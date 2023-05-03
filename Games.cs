using Newtonsoft.Json;
using RetroGamesApi.DTOs;
using System.Collections.Generic;

namespace RetroGamesApi
{
    public class Games
    {
        [JsonProperty("games")]
        public List<GameDTO> GamesList { get; set; }
    }
}