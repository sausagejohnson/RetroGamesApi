using Newtonsoft.Json;
using System.Collections.Generic;

namespace RetroGamesApi.DTOs
{
    public class GameDTO
    {
        [JsonProperty("game_id")]
        public int GameId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("platform_ids")]
        public int[] platformIds { get; set; }

    }

}
