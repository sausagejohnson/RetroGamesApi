using Newtonsoft.Json;
using System.Collections.Generic;

namespace RetroGamesApi.DTOs
{
    //DTO used for saving back from a client
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
