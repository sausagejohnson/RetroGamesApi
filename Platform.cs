using Newtonsoft.Json;
using System;

namespace RetroGamesApi
{
    public class Platform
    {
        [JsonProperty("platform_id")]
        public int PlatformId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

}